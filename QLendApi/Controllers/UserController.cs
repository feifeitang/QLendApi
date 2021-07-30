using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Extensions;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Repositories;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IForeignWorkerRepository foreignWorkerRepository;
        private readonly ICertificateRepository certificateRepository;

        private readonly double _expireMins;

        public UserController(IForeignWorkerRepository foreignWorkerRepository, ICertificateRepository certificateRepository)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;
            this.certificateRepository = certificateRepository;
            this._expireMins = 1.5;
        }

        // POST /api/user/signup
        [Route("signup")]
        [HttpPost]
        public async Task<ActionResult> SignupUser(SignupUserDto signupUser)
        {

            // check UINo if exist
            if (certificateRepository.CheckUINoExist(signupUser.UINo))
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10001,
                    Message = "Exist UINo"
                });
            }

            // check PhoneNumber if exist
            if (foreignWorkerRepository.CheckPhoneNumberExist(signupUser.PhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10002,
                    Message = "Exist Phone Number"
                });
            }

            Certificate certificate = new()
            {
                Uino = signupUser.UINo
            };

            var hashPwd = Crypt.Hash(signupUser.Password);

            ForeignWorker foreignWorker = new()
            {
                PhoneNumber = signupUser.PhoneNumber,
                Password = hashPwd,
                Uino = signupUser.UINo,
                Status = 1,
                RegisterTime = DateTime.UtcNow
            };

            await certificateRepository.CreateCertificateAsync(certificate);
            await foreignWorkerRepository.CreateForeignWorkerAsync(foreignWorker);

            return StatusCode(201);
        }


        // POST /api/user/checkOTP
        [Route("checkOTP")]
        [HttpPost]
        public async Task<ActionResult> CheckOTP(CheckOtpDto checkOtpDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(checkOtpDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                // check send time and compare OTP number
                if (!CheckOTPSendTimeIsVaild(foreignWorker.OTPSendTIme.Value))
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10004,
                        Message = "otp code expire"
                    });
                }

                if (foreignWorker.OTP != checkOtpDto.OTP)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10005,
                        Message = "otp code not equal"
                    });
                }

                foreignWorker.Status = 2;

                await foreignWorkerRepository.UpdateForeignWorkerAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST /api/user/sendOTP
        [Route("sendOTP")]
        [HttpPost]
        public async Task<ActionResult> SendOTP(SendOtpDto sendOtpDto)
        {
            // check user exist, and get user data
            var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(sendOtpDto.Id);

            if (foreignWorker == null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10003,
                    Message = "user not found"
                });
            }

            Random rnd = new Random();
            int OTP = rnd.Next(100000, 999999);

            foreignWorker.OTP = OTP;
            foreignWorker.OTPSendTIme = DateTime.UtcNow;

            await foreignWorkerRepository.UpdateForeignWorkerAsync(foreignWorker);

            return StatusCode(201);
        }

        // POST /api/user/arc
        [Route("arc")]
        [HttpPost]
        public async Task<ActionResult> Arc([FromForm] ArcDto arcDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(arcDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                var cert = await certificateRepository.GetCertificateAsync(foreignWorker.Uino);

                cert.FrontArc = await arcDto.FrontArc.GetBytes();
                cert.BackArc = await arcDto.BackArc.GetBytes();

                await certificateRepository.UpdateCertificateAsync(cert);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90005,
                    Message = $"arc api error:{ex}"
                });
            }
        }

        // POST /api/user/personalInfo
        [Route("personalInfo")]
        [HttpPost]
        public ActionResult PersonalInfo()
        {
            return StatusCode(201);
        }

        private bool CheckOTPSendTimeIsVaild(DateTime sendTime)
        {
            // sendTime need add _expireMins
            var expireTime = sendTime.AddMinutes(this._expireMins);
            DateTime currentTime = DateTime.UtcNow;
            int result = DateTime.Compare(expireTime, currentTime);

            bool res = true;

            if (result < 0)
                res = false;

            return res;
        }
    }
}
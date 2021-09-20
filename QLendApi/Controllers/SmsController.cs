using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Repositories;
using QLendApi.Responses;
using QLendApi.Services;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly IForeignWorkerRepository foreignWorkerRepository;
        private readonly ISmsService smsService;
        private readonly double _expireMins;

        public SmsController(IForeignWorkerRepository foreignWorkerRepository, ISmsService smsService)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;
            this.smsService = smsService;

            this._expireMins = 2;
        }

        // POST /api/sms/otp
        [Route("otp")]
        [HttpPost]
        public async Task<ActionResult> Otp(OtpDto otpDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(otpDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "User not found"
                    });
                }

                Random rnd = new Random();
                int OTP = rnd.Next(100000, 999999);

                smsService.Send(foreignWorker.PhoneNumber, $"QLend OTP number is {OTP}");

                foreignWorker.OTP = OTP;
                foreignWorker.OTPSendTIme = DateTime.UtcNow;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90002,
                    Message = $"sendOtp api error:{ex}"
                });
            }
        }

        // POST /api/sms/OtpByPhoneNumber
        [Route("OtpByPhoneNumber")]
        [HttpPost]
        public async Task<ActionResult> OtpByPhoneNumber(OtpByPhoneNumberDto otpByPhoneNumberDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByPhoneNumberAsync(otpByPhoneNumberDto.PhoneNumber);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "User not found"
                    });
                }

                Random rnd = new Random();
                int OTP = rnd.Next(100000, 999999);

                smsService.Send(foreignWorker.PhoneNumber, $"QLend OTP number is {OTP}");

                foreignWorker.OTP = OTP;
                foreignWorker.OTPSendTIme = DateTime.UtcNow;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return Ok(new OtpByPhoneNumberResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success",
                    Data = new OtpByPhoneNumberResponse.DataStruct
                    {
                        Id = foreignWorker.Id
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90002,
                    Message = $"sendOtp api error:{ex}"
                });
            }
        }

        // POST /api/sms/checkOtp
        [Route("checkOtp")]
        [HttpPost]
        public async Task<ActionResult> CheckOtp(CheckOtpDto checkOtpDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(checkOtpDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "User not found"
                    });
                }

                // check send time and compare OTP number
                if (!CheckOtpSendTimeIsVaild(foreignWorker.OTPSendTIme.Value))
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10004,
                        Message = "Otp code expire"
                    });
                }

                if (foreignWorker.OTP != checkOtpDto.OTP)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10005,
                        Message = "Otp code not equal"
                    });
                }

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90003,
                    Message = $"checkOtp api error:{ex}"
                });
            }
        }

        private bool CheckOtpSendTimeIsVaild(DateTime sendTime)
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
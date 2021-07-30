using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QLendApi.Dtos;
using QLendApi.Helpers;
using QLendApi.Extensions;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Services;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IForeignWorkerRepository foreignWorkerRepository;
        private readonly ICertificateRepository certificateRepository;

        private readonly IForeignWorkerService foreignWorkerService;


        private readonly AppSettings _appSettings;

        private readonly double _expireMins;

        public UserController(
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository,
            IOptions<AppSettings> appSettings,
            IForeignWorkerService foreignWorkerService)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;

            this._appSettings = appSettings.Value;

            this.foreignWorkerService = foreignWorkerService;

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

                // check status
                if (foreignWorker.Status != 2)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10007,
                        Message = "status not correct"
                    });
                }

                var cert = await certificateRepository.GetCertificateAsync(foreignWorker.Uino);

                if (cert == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10006,
                        Message = "certificate not found"
                    });
                }

                cert.FrontArc = await arcDto.FrontArc.GetBytes();
                cert.BackArc = await arcDto.BackArc.GetBytes();

                await certificateRepository.UpdateCertificateAsync(cert);

                foreignWorker.Status = 3;
                await foreignWorkerRepository.UpdateForeignWorkerAsync(foreignWorker);

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
        public async Task<ActionResult> PersonalInfo(PersonalInfoDto personalInfo)
        {
            // check user exist, and get user data
            var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(personalInfo.Id);

            if (foreignWorker == null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10003,
                    Message = "user not found"
                });
            }

            // check user status
            if (foreignWorker.Status != 3)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10007,
                    Message = "status not correct"
                });
            }

            // check certificate exist, and get certificate data
            var certificate = await certificateRepository.GetCertificateAsync(foreignWorker.Uino);

            if (certificate == null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10006,
                    Message = "certificate not found"
                });
            }

            foreignWorker.UserName = personalInfo.UserName;
            foreignWorker.EnglishName = personalInfo.EnglishName;
            foreignWorker.Sex = personalInfo.Gender;
            foreignWorker.Nationality = personalInfo.Nationality;
            certificate.IssueDate = personalInfo.DateOfIssue;
            certificate.ExpiryDate = personalInfo.DateOfExpiry;
            certificate.BarcodeNumber = personalInfo.BarcodeNumber;
            foreignWorker.Status = 4;

            await foreignWorkerRepository.UpdateForeignWorkerAsync(foreignWorker);
            await certificateRepository.UpdateCertificateAsync(certificate);

            return StatusCode(201);
        }

        // POST /api/user/login
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerService.GetInfoByAuthOrNull(loginDto.Id, loginDto.Password);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10008,
                        Message = "account or password error"
                    });
                }

                // need to check user status

                // authentication successful so generate jwt token
                var token = generateJwtToken(foreignWorker);

                return Ok(new LoginResponse
                {
                    StatusCode = 10000,
                    Message = "login success",
                    Token = token
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90006,
                    Message = $"login api error:{ex}"
                });
            }
        }

        // POST /api/user/info
        [Authorize]
        [Route("info")]
        [HttpGet]
        public ActionResult Info()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                return Ok(new GetForeignWorkerInfoResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                    Info = foreignWorker
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90007,
                    Message = $"get user info api error:{ex}"
                });
            }

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

        private string generateJwtToken(ForeignWorker foreignWorker)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", foreignWorker.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
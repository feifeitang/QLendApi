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
        private readonly IIncomeInformationRepository incomeInformationRepository;
        private readonly ILoanRecordRepository loanRecordRepository;

       static int sn = 0;  

    

        private readonly AppSettings _appSettings;
        private readonly double _expireMins;

        public UserController(
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository,
            IIncomeInformationRepository incomeInformationRepository,
            ILoanRecordRepository loanRecordRepository,
            IOptions<AppSettings> appSettings,
            IForeignWorkerService foreignWorkerService)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;

            this.incomeInformationRepository = incomeInformationRepository;

            this.loanRecordRepository = loanRecordRepository;

            this._appSettings = appSettings.Value;

            this.foreignWorkerService = foreignWorkerService;

            this._expireMins = 1.5;
        }

        // POST /api/user/signUp
        [Route("signUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto signupUser)
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

            await certificateRepository.CreateAsync(certificate);
            await foreignWorkerRepository.CreateAsync(foreignWorker);

            return StatusCode(201);
        }


        // POST /api/user/checkOtp
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

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90002,
                    Message = $"checkOTP api error:{ex}"
                });
            }
        }

        // POST /api/user/sendOtp
        [Route("sendOtp")]
        [HttpPost]
        public async Task<ActionResult> SendOtp(SendOtpDto sendOtpDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(sendOtpDto.Id);

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

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90003,
                    Message = $"sendOTP api error:{ex}"
                });
            }
        }

        // POST /api/user/initArc
        [Route("initArc")]
        [HttpPost]
        public async Task<ActionResult> InitArc([FromForm] ArcDto arcDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(arcDto.Id);

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

                var cert = await certificateRepository.GetByUINoAsync(foreignWorker.Uino);

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

                await certificateRepository.UpdateAsync(cert);

                foreignWorker.Status = 3;
                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90004,
                    Message = $"arc api error:{ex}"
                });
            }
        }

        // POST /api/user/personalInfo
        [Route("personalInfo")]
        [HttpPost]
        public async Task<ActionResult> PersonalInfo(PersonalInfoDto personalInfoDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(personalInfoDto.Id);

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

                foreignWorker.UserName = personalInfoDto.UserName;
                foreignWorker.EnglishName = personalInfoDto.EnglishName;
                foreignWorker.Sex = personalInfoDto.Sex;
                foreignWorker.Nationality = personalInfoDto.Nationality;
                foreignWorker.BirthDate = personalInfoDto.BirthDate;
                foreignWorker.PassportNumber = personalInfoDto.PassportNumber;

                foreignWorker.Status = 4;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90005,
                    Message = $"personalInfo api error:{ex}"
                });
            }
        }


        // POST /api/user/arcInfo
        [Route("arcInfo")]
        [HttpPost]
        public async Task<ActionResult> ArcInfo(ArcInfoDto arcInfoDto)
        {
            try
            {
                // check user exist, and get user data
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(arcInfoDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                // check user status
                if (foreignWorker.Status != 4)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10007,
                        Message = "status not correct"
                    });
                }

                // check certificate exist, and get certificate data
                var certificate = await certificateRepository.GetByUINoAsync(foreignWorker.Uino);

                if (certificate == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10006,
                        Message = "certificate not found"
                    });
                }

                certificate.IssueDate = arcInfoDto.DateOfIssue;
                certificate.ExpiryDate = arcInfoDto.DateOfExpiry;
                certificate.BarcodeNumber = arcInfoDto.BarcodeNumber;
                foreignWorker.KindOfWork = arcInfoDto.KindOfWork;
                foreignWorker.Workplace = arcInfoDto.Workplace;

                foreignWorker.Status = 5;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                await certificateRepository.UpdateAsync(certificate);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90005,
                    Message = $"personalInfo api error:{ex}"
                });
            }
        }

        // POST /api/user/login
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerService.GetInfoByAuthOrNull(loginDto.Uino, loginDto.Password);

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

        //POST /api/user/loanSurveyInfo
        [Authorize]
        [Route("loanSurveyInfo")]
        [HttpPost]
        public async Task<ActionResult> LoanSurveyInfo(LoanSurveyInfoDto loanSurveyInfoDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                foreignWorker.Marriage = loanSurveyInfoDto.Marriage;
                foreignWorker.ImmediateFamilyNumber = loanSurveyInfoDto.ImmediateFamilyNumber;
                foreignWorker.EducationBackground = loanSurveyInfoDto.EducationBackground;
                foreignWorker.TimeInTaiwan = loanSurveyInfoDto.TimeInTaiwan;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90008,
                    Message = $"user info update api error:{ex}"
                });
            }

        }


        //POST /api/user/incomeInfo
        [Route("incomeInfo")]
        [HttpPost]
        public async Task<ActionResult> IncomeInfo([FromForm] IncomeInfoDto incomeInfoDto)
        {           
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                //check incomeNumber if exist
                if(foreignWorker.IncomeNumber != null)
                {
                    var incomeInfo = await incomeInformationRepository.GetByIncomeNumberAsync(foreignWorker.IncomeNumber);
                                                               
                    incomeInfo.AvgMonthlyIncome = incomeInfoDto.AvgMonthlyIncome;
                    incomeInfo.LatePay = incomeInfoDto.LatePay;
                    incomeInfo.PayWay = incomeInfoDto.PayWay;
                    incomeInfo.RemittanceWay = incomeInfoDto.RemittanceWay;

                    if(incomeInfoDto.FrontSalaryPassbook != null && incomeInfoDto.InsideSalarybook != null)
                    {
                        incomeInfo.FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes();
                        incomeInfo.InsideSalarybook = await incomeInfoDto.InsideSalarybook.GetBytes(); 
                    }
                    if(incomeInfoDto.FrontSalaryPassbook == null && incomeInfoDto.InsideSalarybook != null)
                    {
                        incomeInfo.FrontSalaryPassbook = null;
                        incomeInfo.InsideSalarybook = await incomeInfoDto.InsideSalarybook.GetBytes();
                    }
                    if(incomeInfoDto.FrontSalaryPassbook != null && incomeInfoDto.InsideSalarybook == null)
                    {
                        incomeInfo.FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes();
                        incomeInfo.InsideSalarybook = null;
                    }
                    if(incomeInfoDto.FrontSalaryPassbook == null && incomeInfoDto.InsideSalarybook == null)
                    {
                        incomeInfo.FrontSalaryPassbook = null;
                        incomeInfo.InsideSalarybook = null;
                    }

                    await incomeInformationRepository.UpdateAsync(incomeInfo);                                                                                           
                }
                else
                {
                    //if incomeNumber doesn't exist                   
                    IncomeInformation incomeInformation = new()
                    {
                        AvgMonthlyIncome = incomeInfoDto.AvgMonthlyIncome,
                        LatePay = incomeInfoDto.LatePay,
                        PayWay = incomeInfoDto.PayWay,
                        RemittanceWay = incomeInfoDto.RemittanceWay                     
                    };

                    if(incomeInfoDto.FrontSalaryPassbook != null && incomeInfoDto.InsideSalarybook != null)
                    {
                        incomeInformation.FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes();
                        incomeInformation.InsideSalarybook = await incomeInfoDto.InsideSalarybook.GetBytes(); 
                    }
                    if(incomeInfoDto.FrontSalaryPassbook == null && incomeInfoDto.InsideSalarybook != null)
                    {
                        incomeInformation.FrontSalaryPassbook = null;
                        incomeInformation.InsideSalarybook = await incomeInfoDto.InsideSalarybook.GetBytes();
                    }
                    if(incomeInfoDto.FrontSalaryPassbook != null && incomeInfoDto.InsideSalarybook == null)
                    {
                        incomeInformation.FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes();
                        incomeInformation.InsideSalarybook = null;
                    }
                    if(incomeInfoDto.FrontSalaryPassbook == null && incomeInfoDto.InsideSalarybook == null)
                    {
                        incomeInformation.FrontSalaryPassbook = null;
                        incomeInformation.InsideSalarybook = null;
                    }
                    
                    incomeInformation.IncomeNumber = GenerateIncomeNumber();
                    
                    await incomeInformationRepository.CreateAsync(incomeInformation); 

                    foreignWorker.IncomeNumber = incomeInformation.IncomeNumber;
                    await foreignWorkerRepository.UpdateAsync(foreignWorker);  
                }                                                               
                                                                                                            
                return StatusCode(201);                      
               
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90010,
                    Message = $"incomeInfo api error:{ex}"
                });
            }
          
        }

        // POST /api/user/loanSurveyArc
        [Authorize]
        [Route("loanSurveyArc")]
        [HttpPost]
        public async Task<ActionResult> loanSurveyArc([FromForm] LoanSurveyArcDto loanSurveyArcDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;
 /*               
                var loanRecord = await loanRecordRepository.GetByIdAndStateAsync(foreignWorker.Id);                

                 // check user status
                if (loanRecord.State != 0)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10007,
                        Message = "status not correct"
                    });             
                }
*/
                var cert = await certificateRepository.GetByUINoAsync(foreignWorker.Uino); 

                cert.FrontArc2 = await loanSurveyArcDto.FrontArc2.GetBytes();
                cert.BackArc2 = await loanSurveyArcDto.BackArc2.GetBytes();
                cert.SelfileArc = await loanSurveyArcDto.SelfieArc.GetBytes();    

                await certificateRepository.UpdateAsync(cert); 
/*                
                loanRecord.State = 1;
                await loanRecordRepository.UpdateAsync(loanRecord);
*/
                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90040,
                    Message = $"arcSelfie api error:{ex}"
                });
            }
        }

        // POST /api/user/signature
        [Authorize]
        [Route("signature")]
        [HttpPost]
        public async Task<ActionResult> Signature([FromForm] SignatureDto signatureDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                foreignWorker.Signature = await signatureDto.Signature.GetBytes();

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90050,
                    Message = $"signature api error:{ex}"
                });
            }
        }

        // POST /api/user/loanConfirmSignature
        [Authorize]
        [Route("loanConfirmSignature")]
        [HttpPost]
        public async Task<ActionResult> LoanConfirmSignature([FromForm] LoanConfirmSignatureDto loanConfirmSignatureDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                foreignWorker.Signature2 = await loanConfirmSignatureDto.Signature2.GetBytes();

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90050,
                    Message = $"signature api error:{ex}"
                });
            }
        }

        // POST /api/user/bankAccount
        [Route("bankAccount")]
        [HttpPost]
        public async Task<ActionResult> BankAccount(BankAccountDto bankAccountDto)
        {
            try
            {
                //get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                foreignWorker.BankNumber = bankAccountDto.BankNumber;
                foreignWorker.AccountNumber = bankAccountDto.AccountNumber;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90060,
                    Message = $"bankAccount api error:{ex}"
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

        public static int GenerateIncomeNumber()
        {     
               
            int number = int.Parse(DateTime.UtcNow.ToString("yyMMdd") + string.Format("{0:d4}", sn));                                               
            sn ++; 
            return number;
        }

        
    }
}
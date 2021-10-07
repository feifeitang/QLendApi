using System;
using System.IdentityModel.Tokens.Jwt;
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
using QLendApi.Responses;
using QLendApi.Settings;
using System.Threading;

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
        private readonly INoticeRepository noticeRepository;
        private readonly INotificationService _notificationService;
        private readonly ISmsService smsService;

        private readonly AppSettings _appSettings;

        private int sn = 0;

        public UserController(
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository,
            IIncomeInformationRepository incomeInformationRepository,
            ILoanRecordRepository loanRecordRepository,
            IOptions<AppSettings> appSettings,
            IForeignWorkerService foreignWorkerService,
            INoticeRepository noticeRepository,
            INotificationService _notificationService,
            ISmsService smsService)
        {
            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;

            this.incomeInformationRepository = incomeInformationRepository;

            this.loanRecordRepository = loanRecordRepository;

            this.noticeRepository = noticeRepository;

            this._appSettings = appSettings.Value;

            this.foreignWorkerService = foreignWorkerService;

            this._notificationService = _notificationService;
        }

        // POST /api/user/signUp
        [Route("signUp")]
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpDto signUp)
        {
            try
            {
                // check UINo if exist
                if (certificateRepository.CheckUINoExist(signUp.UINo))
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10001,
                        Message = "exist uino"
                    });
                }

                // check PhoneNumber if exist
                if (foreignWorkerRepository.CheckPhoneNumberExist(signUp.PhoneNumber))
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10002,
                        Message = "exist phone number"
                    });
                }

                Certificate certificate = new()
                {
                    Uino = signUp.UINo
                };

                var hashPwd = Crypt.Hash(signUp.Password);

                ForeignWorker foreignWorker = new()
                {
                    PhoneNumber = signUp.PhoneNumber,
                    Password = hashPwd,
                    Uino = signUp.UINo,
                    Status = ForeignWorkStatus.Init,
                    RegisterTime = DateTime.UtcNow
                };

                await certificateRepository.CreateAsync(certificate);
                await foreignWorkerRepository.CreateAsync(foreignWorker);

                return Ok(new SignUpResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success",
                    Data = new SignUpResponse.DataStruct
                    {
                        Id = foreignWorker.Id
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90001,
                    Message = $"signUp api error:{ex}"
                });
            }
        }

        // POST /api/user/initArc
        [Route("initArc")]
        [HttpPost]
        public async Task<ActionResult> PersonalNationalInfo([FromForm] PersonalNationalInfoDto personalNationalInfoDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerRepository.GetByIdAsync(personalNationalInfoDto.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                // check status
                if (foreignWorker.Status != ForeignWorkStatus.Init)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10004,
                        Message = "status not correct"
                    });
                }

                var cert = await certificateRepository.GetByUINoAsync(foreignWorker.Uino);

                if (cert == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10005,
                        Message = "certificate not found"
                    });
                }

                cert.FrontArc = await personalNationalInfoDto.FrontArc.GetBytes();
                cert.BackArc = await personalNationalInfoDto.BackArc.GetBytes();
                foreignWorker.Passport = await personalNationalInfoDto.Passport.GetBytes();
                foreignWorker.LocalIdCard = await personalNationalInfoDto.LocalIdCard.GetBytes();

                await certificateRepository.UpdateAsync(cert);

                foreignWorker.Status = ForeignWorkStatus.InitArcFinish;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90002,
                    Message = $"initArc api error:{ex}"
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
                if (foreignWorker.Status != ForeignWorkStatus.InitArcFinish)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10004,
                        Message = "status not correct"
                    });
                }

                foreignWorker.UserName = personalInfoDto.UserName;
                foreignWorker.EnglishName = personalInfoDto.EnglishName;
                foreignWorker.Sex = personalInfoDto.Sex;
                foreignWorker.Nationality = personalInfoDto.Nationality;
                foreignWorker.BirthDate = personalInfoDto.BirthDate;
                foreignWorker.FamilyMemberName = personalInfoDto.FamilyMemberName;
                foreignWorker.FamilyMemberPhoneNumber = personalInfoDto.FamilyMemberPhoneNumber;
                foreignWorker.FacebookAccount = personalInfoDto.FacebookAccount;
                foreignWorker.CommunicationSoftware = personalInfoDto.CommunicationSoftware;
                foreignWorker.CommunicationSoftwareAccount = personalInfoDto.CommunicationSoftwareAccount;

                foreignWorker.Status = ForeignWorkStatus.PersonalInfoFinish;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                
                //return StatusCode(201);
                return Ok(new BaseResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90003,
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
                if (foreignWorker.Status != ForeignWorkStatus.PersonalInfoFinish)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10004,
                        Message = "status not correct"
                    });
                }

                // check certificate exist, and get certificate data
                var certificate = await certificateRepository.GetByUINoAsync(foreignWorker.Uino);

                if (certificate == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10005,
                        Message = "certificate not found"
                    });
                }

                certificate.IssueDate = arcInfoDto.DateOfIssue;
                certificate.ExpiryDate = arcInfoDto.DateOfExpiry;
                certificate.BarcodeNumber = arcInfoDto.BarcodeNumber;
                foreignWorker.KindOfWork = arcInfoDto.KindOfWork;
                foreignWorker.Workplace = arcInfoDto.Workplace;

                foreignWorker.Status = ForeignWorkStatus.Finish;
                foreignWorker.State = ForeignWorkState.Pending;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                await certificateRepository.UpdateAsync(certificate);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90006,
                    Message = $"arcInfo api error:{ex}"
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
                        StatusCode = 10006,
                        Message = "account or password error"
                    });
                }
              

                var foreignWorkerState = (int)(foreignWorker.State == null ? ForeignWorkState.Failure : foreignWorker.State);

                var checkIsApprove = this.foreignWorkerService.CheckSignupIsApprove(foreignWorkerState);

                if (!checkIsApprove)
                {
                    var checkIsFinishResult = this.foreignWorkerService.CheckSignupIsFinish(foreignWorker.Status);

                    if (!checkIsFinishResult)
                    {
                        return BadRequest(new NotFinishSignupResponse
                        {
                            StatusCode = 10007,
                            Message = "sign up process not finish",
                            Data = new NotFinishSignupResponse.DataStruct
                            {
                                NextStatus = foreignWorker.Status + 1,
                                ForeignWorkerId = foreignWorker.Id,
                            }
                        });
                    }

                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10008,
                        Message = "wait for approve"
                    });
                }


                // authentication successful so generate jwt token
                var token = generateJwtToken(foreignWorker);

                return Ok(new LoginResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "login success",
                    Data = new LoginResponse.DataStruct
                    {
                        Token = token
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90005,
                    Message = $"login api error:{ex}"
                });
            }
        }

        // POST /api/user/password
        [Route("password")]
        [HttpPost]
        public async Task<ActionResult> Password(PasswordDto passwordDto)
        {
            try
            {
                var foreignWorker = await foreignWorkerRepository.GetByPhoneNumberAsync(passwordDto.PhoneNumber);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                var hashPwd = Crypt.Hash(passwordDto.Password);

                foreignWorker.Password = hashPwd;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                //return StatusCode(201);
                return Ok(new BaseResponse
                {
                    StatusCode = 201,
                    Message = "success"                   
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90006,
                    Message = $"password api error:{ex}"
                });
            }
        }

        // GET /api/user/info
        [Authorize]
        [Route("info")]
        [HttpGet]
        public ActionResult Info()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                return Ok(new ForeignWorkerInfoResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success",
                    Data = new ForeignWorkerInfoResponse.DataStruct
                    {
                        Info = foreignWorker
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90007,
                    Message = $"info api error:{ex}"
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

                var loanRecord = await loanRecordRepository.GetByLoanNumber(loanSurveyInfoDto.LoanNumber);

                loanRecord.State = LoanState.LoanSurveyInfoFinish;
                loanRecord.CreateTime = DateTime.UtcNow;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                await loanRecordRepository.UpdateAsync(loanRecord);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90008,
                    Message = $"loanSurveyInfo api error:{ex}"
                });
            }

        }

        //POST /api/user/incomeInfo
        [Authorize]
        [Route("incomeInfo")]
        [HttpPost]
        public async Task<ActionResult> IncomeInfo([FromForm] IncomeInfoDto incomeInfoDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                // if incomeNumber exist
                if (foreignWorker.IncomeNumber != null)
                {
                    var incomeInfo = await incomeInformationRepository.GetByIncomeNumberAsync(foreignWorker.IncomeNumber);

                    incomeInfo.AvgMonthlyIncome = incomeInfoDto.AvgMonthlyIncome;
                    incomeInfo.LatePay = incomeInfoDto.LatePay;
                    incomeInfo.PayWay = incomeInfoDto.PayWay;
                    incomeInfo.RemittanceWay = incomeInfoDto.RemittanceWay;
                    incomeInfo.FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes();
                    incomeInfo.PaySlip = await incomeInfoDto.PaySlip.GetBytes();
                    incomeInfo.PayDay = incomeInfoDto.PayDay;

                    await incomeInformationRepository.UpdateAsync(incomeInfo);
                }
                // if incomeNumber not exist
                else
                {
                    IncomeInformation incomeInfo = new()
                    {
                        IncomeNumber = GenerateIncomeNumber(),
                        AvgMonthlyIncome = incomeInfoDto.AvgMonthlyIncome,
                        LatePay = incomeInfoDto.LatePay,
                        PayWay = incomeInfoDto.PayWay,
                        RemittanceWay = incomeInfoDto.RemittanceWay,
                        FrontSalaryPassbook = await incomeInfoDto.FrontSalaryPassbook.GetBytes(),
                        PaySlip = await incomeInfoDto.PaySlip.GetBytes()
                    };

                    foreignWorker.IncomeNumber = incomeInfo.IncomeNumber;

                    await incomeInformationRepository.CreateAsync(incomeInfo);
                    await foreignWorkerRepository.UpdateAsync(foreignWorker);

                }

                var loanRecord = await loanRecordRepository.GetByLoanNumber(incomeInfoDto.LoanNumber);
       
                loanRecord.State = LoanState.IncomeInfoFinish;
                loanRecord.CreateTime = DateTime.UtcNow;

                await loanRecordRepository.UpdateAsync(loanRecord);

                return StatusCode(201);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90009,
                    Message = $"incomeInfo api error:{ex}"
                });
            }

        }

        // POST /api/user/loanSurveyArc
        [Authorize]
        [Route("loanSurveyArc")]
        [HttpPost]
        public async Task<ActionResult> LoanSurveyArc([FromForm] LoanSurveyArcDto loanSurveyArcDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var cert = await certificateRepository.GetByUINoAsync(foreignWorker.Uino);

                cert.FrontArc2 = await loanSurveyArcDto.FrontArc2.GetBytes();
                cert.BackArc2 = await loanSurveyArcDto.BackArc2.GetBytes();
                cert.SelfileArc = await loanSurveyArcDto.SelfieArc.GetBytes();

                var loanRecord = await loanRecordRepository.GetByLoanNumber(loanSurveyArcDto.LoanNumber);

                loanRecord.State = LoanState.LoanSurveyArcFinish;
                loanRecord.CreateTime = DateTime.UtcNow;

                await certificateRepository.UpdateAsync(cert);
                await loanRecordRepository.UpdateAsync(loanRecord);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90010,
                    Message = $"loanSurveyArc api error:{ex}"
                });
            }
        }

        // POST /api/user/loanApplySignature
        [Authorize]
        [Route("loanApplySignature")]
        [HttpPost]
        public async Task<ActionResult> LoanApplySignature([FromForm] LoanApplySignatureDto loanApplySignatureDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                foreignWorker.Signature = await loanApplySignatureDto.Signature.GetBytes();

                var loanRecord = await loanRecordRepository.GetByLoanNumber(loanApplySignatureDto.LoanNumber);

                loanRecord.State = LoanState.ApplyFinish;
                loanRecord.CreateTime = DateTime.UtcNow;

                var Content = "We haved received your application. Please wait for the result.";

                Notice notice = new()
                {
                    Content = Content,
                    Status = NoticeStatus.Success,
                    Link = null,
                    CreateTime = DateTime.UtcNow,
                    ForeignWorkerId = foreignWorker.Id
                };

                await foreignWorkerRepository.UpdateAsync(foreignWorker);

                await loanRecordRepository.UpdateAsync(loanRecord);

                await noticeRepository.CreateAsync(notice);

                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                NotificationRequest notificationRequest = new()
                {
                    Title = "QLend",
                    Text = Content,
                    Action = Content,
                    Tags = new string[] { foreignWorker.DeviceTag },
                    Silent = false
                };

                await _notificationService.RequestNotificationAsync(notificationRequest, token);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90020,
                    Message = $"loanApplySignature api error:{ex}"
                });
            }
        }

         // GET /api/user/getIncomeinfo
        [Authorize]
        [Route("getIncomeinfo")]
        [HttpGet]
        public async Task<ActionResult> GetIncomeInfo()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;
                var incomeInfo = await incomeInformationRepository.GetByIncomeNumberAsync(foreignWorker.IncomeNumber);

                return Ok(new IncomeInfoResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success",
                    Data = new IncomeInfoResponse.DataStruct
                    {
                        PayDay = incomeInfo.PayDay
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90007,
                    Message = $"info api error:{ex}"
                });
            }
        }

        // POST /api/user/bankAccount
        [Authorize]
        [Route("bankAccount")]
        [HttpPost]
        public async Task<ActionResult> BankAccount(BankAccountDto bankAccountDto)
        {
            try
            {
                //get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await loanRecordRepository.GetByLoanNumber(bankAccountDto.LoanNumber);

                if (foreignWorker.Id != loanRecord.Id)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10009,
                        Message = "id not equal"
                    });
                }

                if (loanRecord.State != LoanState.ConfirmLoan)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10010,
                        Message = "loan state not correct"
                    });
                }

                foreignWorker.BankNumber = bankAccountDto.BankNumber;
                foreignWorker.AccountNumber = bankAccountDto.AccountNumber;
                loanRecord.State = LoanState.BankAccountFinish;
                //loanRecord.Status = 1;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                await loanRecordRepository.UpdateAsync(loanRecord);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90030,
                    Message = $"bankAccount api error:{ex}"
                });
            }
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

        private int GenerateIncomeNumber()
        {
            // Random rnd = new Random();           
            int number = int.Parse(DateTime.UtcNow.ToString("yyddHHss") + string.Format("{0:d2}", sn));
            sn++;
            return number;
        }
    }
}
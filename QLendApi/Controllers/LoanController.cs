using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Extensions;
using QLendApi.Repositories;
using System;

namespace QLendApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRecordRepository loanRecordRepository;
        private readonly IForeignWorkerRepository foreignWorkerRepository;
        private readonly ICertificateRepository certificateRepository;
        private readonly IRepaymentRecordRepository repaymentRecordRepository;
        static int sn = 0;

        public LoanController(
            ILoanRecordRepository loanRecordRepository,
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository,
            IRepaymentRecordRepository repaymentRecordRepository)
        {
            this.loanRecordRepository = loanRecordRepository;

            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;

            this.repaymentRecordRepository = repaymentRecordRepository;
        }

        //POST /api/loan/apply
        [Route("apply")]
        [HttpPost]
        public async Task<ActionResult> Apply(LoanApplyDto loanApply)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var  loanRecord = await loanRecordRepository.GetByIdAndStateAsync(foreignWorker.Id, 0);

                if(loanRecord  != null)
                {
                    loanRecord.Amount = loanApply.Amount;
                    loanRecord.Period = loanApply.Period;
                    loanRecord.Purpose = loanApply.Purpose;
                    loanRecord.Id = foreignWorker.Id;
                    loanRecord.State = 0;

                    await loanRecordRepository.UpdateAsync(loanRecord);
                }
                else
                {
                    LoanRecord loanRecordInfo = new()
                    {
                        Amount = loanApply.Amount,
                        Period = loanApply.Period,
                        Purpose = loanApply.Purpose,
                        Id = foreignWorker.Id,
                        State = 0                 
                    };

                    if(foreignWorker.Nationality == null)
                    {
                        return BadRequest(new BaseResponse
                        {
                            StatusCode = 10509,
                            Message = "nationaality is null"
                        });
                    }
                
                    loanRecordInfo.LoanNumber = GenerateLoanNumber(foreignWorker.Nationality);
                    await loanRecordRepository.CreateAsync(loanRecordInfo);     
                }           
                                                                          
                                   

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90009,
                    Message = $"loan apply api error:{ex}"
                });
            }           
        }     

        // POST /api/loan/success
        [Route("success")]
        [HttpPost]
        public ActionResult success()
        {
            return StatusCode(201);
        }

        //GET /api/loan/loanData
        [Route("loanData")]
        [HttpGet]
        public async Task<ActionResult<GetLoanDataResponseDto>> LoanData()
        {
           try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

              ///  var loanRecord = await loanRecordRepository.GetByIdAsync(foreignWorker.Id);
                                
                var loanRecord = await loanRecordRepository.GetByIdAndStateAsync(foreignWorker.Id,3);              

                if(loanRecord == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10350,
                        Message = "haven't loanRecord."
                    });
                }
                else
                {
                     return Ok(new GetLoanDataResponseDto
                    {
                        StatusCode = 10000,
                        Message = "success",
                        UserName = foreignWorker.UserName,
                        Amount = loanRecord.Amount,
                        Period = loanRecord.Period
                    });
                }
               
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90009,
                    Message = $"loanData api error:{ex}"
                });
            }
        }

        //GET api/loan/applySuccess
        [Route("applySuccess")]
        [HttpGet]
        public  async Task<ActionResult<GetLoanApplySuccessDto>> LoanApplySuccess()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;
                var loanRecord = await loanRecordRepository.GetByIdAndStateAsync(foreignWorker.Id,4);

                if(loanRecord == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10350,
                        Message = "haven't loanRecord."
                    });
                }
                else
                {
                    loanRecord.State = 5;
                    await loanRecordRepository.UpdateAsync(loanRecord);

                    return Ok(new GetLoanApplySuccessDto
                    {
                        StatusCode = 10000,
                        Message = "success",
                        Amount = loanRecord.Amount,
                        Period = loanRecord.Period
                    });   
                }                
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90036,
                    Message = $"applySuccess api error:{ex}"
                });
            }
        }

        // GET /api/loan/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<GetLoanListResponseDto>> list(int status)
        {
            var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

            var loanRecord = await this.loanRecordRepository.GetByIdAndStatusAsync(foreignWorker.Id, status);

            return Ok(new GetLoanListResponseDto
            {
                StatusCode = 10000,
                Message = "success",
                LoanRecords = loanRecord
            });
        }

        // GET /api/loan/detail/{loanNumber}
        [Route("detail/{loanNumber}")]
        [HttpGet]
        public async Task<ActionResult<GetLoanDetailResponseDto>> Detail(string loanNumber)
        {
            var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

            var loanRecord = await this.loanRecordRepository.GetByLoanNumber(loanNumber);

            if (foreignWorker.Id != loanRecord.Id)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10009,
                    Message = "id not equal"
                });
            }

            var repaymentRecords = await this.repaymentRecordRepository.GetByLoanNumberAsync(loanNumber);

            return Ok(new GetLoanDetailResponseDto{
                StatusCode = 10000,
                Message = "success",
                LoanRecord = loanRecord,
                RepaymentRecords = repaymentRecords
            });
        }

        private string GenerateLoanNumber(string nationality)
        {          
            string number = nationality.Substring(0,1) + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", sn);                                               
            sn ++; 
            return number;
        }
    }
}
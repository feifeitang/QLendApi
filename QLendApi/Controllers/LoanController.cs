using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.Models;
using QLendApi.Repositories;
using System;
using QLendApi.Services;
using QLendApi.Responses;
using QLendApi.lib;
using QLendApi.Extensions;

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
        private readonly ILoanRecordService loanRecordService;
        static int sn = 0;
        // private readonly double _expireHours;

        public LoanController(
            ILoanRecordRepository loanRecordRepository,
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository,
            IRepaymentRecordRepository repaymentRecordRepository,
            ILoanRecordService loanRecordService)
        {
            this.loanRecordRepository = loanRecordRepository;

            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;

            this.repaymentRecordRepository = repaymentRecordRepository;

            this.loanRecordService = loanRecordService;
        }

        //GET /api/loan/editRecord
        [Route("editRecord")]
        [HttpGet]
        public async Task<ActionResult<EditRecordResponse>> EditRecord()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await loanRecordService.GetEditRecordByForeignWorkerId(foreignWorker.Id);

                return Ok(new EditRecordResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                    Data = new EditRecordResponse.EditRecordDataStruct
                    {
                        LoanRecord = loanRecord
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90040,
                    Message = $"editRecord api error:{ex}"
                });
            }
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

                if (foreignWorker.Nationality == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10020,
                        Message = "nationaality is null"
                    });
                }

                LoanRecord loanRecord = new()
                {
                    LoanNumber = GenerateLoanNumber(foreignWorker.Nationality),
                    Amount = loanApply.Amount,
                    Period = loanApply.Period,
                    Purpose = loanApply.Purpose,
                    State = LoanState.ApplyInit,
                    Id = foreignWorker.Id,
                    CreateTime = DateTime.UtcNow
                };

                await loanRecordRepository.CreateAsync(loanRecord);

                return Ok(new LoanApplyResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                    Data = new LoanApplyResponse.LoanApplyDataStruct
                    {
                        LoanNumber = loanRecord.LoanNumber
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90050,
                    Message = $"loan apply api error:{ex}"
                });
            }
        }

        // POST /api/loan/confirm
        [Authorize]
        [Route("confirm")]
        [HttpPost]
        public async Task<ActionResult> Confirm([FromForm] ConfirmDto confirmDto)
        {
            try
            {
                // get user info
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await loanRecordRepository.GetByLoanNumber(confirmDto.LoanNumber);

                if (foreignWorker.Id != loanRecord.Id)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10009,
                        Message = "id not equal"
                    });
                }

                if (loanRecord.State != LoanState.PermitLoan)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10010,
                        Message = "loan state not correct"
                    });
                }

                foreignWorker.Signature2 = await confirmDto.Signature2.GetBytes();
                loanRecord.State = LoanState.ConfirmLoan;

                await foreignWorkerRepository.UpdateAsync(foreignWorker);
                await loanRecordRepository.UpdateAsync(loanRecord);

               // return StatusCode(201);
               return Ok(new BaseResponse
                {
                    StatusCode = ResponseStatusCode.Success,
                    Message = "success"
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90060,
                    Message = $"loan confirm api error:{ex}"
                });
            }
        }

        // POST /api/loan/cancel
        [Route("cancel")]
        [HttpPost]
        public async Task<ActionResult> Cancel(LoanCancelDto loanCancelDto)
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await this.loanRecordRepository.GetByLoanNumber(loanCancelDto.LoanNumber);

                if (foreignWorker.Id != loanRecord.Id)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10009,
                        Message = "id not equal"
                    });
                }

                loanRecord.State = LoanState.CancelLoan;

                await loanRecordRepository.UpdateAsync(loanRecord);

                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90070,
                    Message = $"loan cancel api error:{ex}"
                });
            }
        }

        // GET /api/loan/list/{status}
        [Route("list/{status}")]
        [HttpGet]
        public async Task<ActionResult<LoanListResponse>> list(int status)
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await this.loanRecordRepository.GetByForeignWorkerIdAndStatusAsync(foreignWorker.Id, status);

                return Ok(new LoanListResponse
                {
                    
                                      
                    StatusCode = 10000,
                    Message = "success",
                    
                    Data = new LoanListResponse.LoanListDataStruct
                    {                       
                        LoanRecords = loanRecord
                        
                    }
                    
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90080,
                    Message = $"loan list api error:{ex}"
                });
            }
        }

        // GET /api/loan/detail/{loanNumber}
        [Route("detail/{loanNumber}")]
        [HttpGet]
        public async Task<ActionResult<LoanDetailResponse>> Detail(string loanNumber)
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await this.loanRecordRepository.GetByLoanNumber(loanNumber);

                if (foreignWorker.Id != loanRecord.Id)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10090,
                        Message = "id not equal"
                    });
                }

                var repaymentRecords = await this.repaymentRecordRepository.GetByLoanNumberAsync(loanNumber);

                return Ok(new LoanDetailResponse
                {
                    StatusCode = 10000,
                    Message = "success",
                    Data = new LoanDetailResponse.LoanDetailDataStruct
                    {
                        LoanRecord = loanRecord,
                        RepaymentRecords = repaymentRecords
                    }
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90100,
                    Message = $"loan detail api error:{ex}"
                });
            }
        }

        private string GenerateLoanNumber(string nationality)
        {
            string number = nationality.Substring(0, 1) + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", sn);
            sn++;
            return number;
        }
    }
}
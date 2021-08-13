using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Extensions;
using QLendApi.Repositories;
using System;
using QLendApi.Services;

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
        public async Task<ActionResult<GetLoanEditRecordResponseDto>> EditRecord()
        {
            try
            {
                var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

                var loanRecord = await loanRecordService.GetEditRecordByForeignWorkerId(foreignWorker.Id);

                return Ok(new GetLoanEditRecordResponseDto
                {
                    StatusCode = 10000,
                    Message = "success",
                    LoanRecord = loanRecord
                });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90070,
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

                if(foreignWorker.Nationality == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10010,
                        Message = "nationaality is null"
                    });
                }

                LoanRecord loanRecord = new()
                {
                    LoanNumber = GenerateLoanNumber(foreignWorker.Nationality),
                    Amount = loanApply.Amount,
                    Period = loanApply.Period,
                    Purpose = loanApply.Purpose,
                    State = 1,
                    Id = foreignWorker.Id,
                    CreateTime = DateTime.UtcNow
                };

                await loanRecordRepository.CreateAsync(loanRecord);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90080,
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


        // GET /api/loan/list
        [Route("list")]
        [HttpGet]
        public async Task<ActionResult<GetLoanListResponseDto>> list(int status)
        {
            var foreignWorker = this.HttpContext.Items["ForeignWorker"] as ForeignWorker;

            var loanRecord = await this.loanRecordRepository.GetByForeignWorkerIdAndStatusAsync(foreignWorker.Id, status);

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
                    StatusCode = 10030,
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
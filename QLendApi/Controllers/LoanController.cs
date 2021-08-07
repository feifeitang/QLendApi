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

                LoanRecord loanRecord = new()
                {
                    Amount = loanApply.Amount,
                    Period = loanApply.Period,
                    Purpose = loanApply.Purpose,
                    Id = foreignWorker.Id
                    //State = 0,
                };                                  
                
                if(foreignWorker.Nationality == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10509,
                        Message = "nationaality is null"
                    });
                }
                
                loanRecord.LoanNumber = GenerateLoanNumber(foreignWorker.Nationality);
                await loanRecordRepository.CreateAsync(loanRecord);                        

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
/*
        //GET /api/loan/loanData
        [Route("loanData")]
        [HttpGet]
        public async Task<ActionResult<GetLoanDataResponseDto>> loanData(int id)
        {
            //get user info
            var foreignWorker = this.HttpContext.Items["forwignWorker"] as ForeignWorker;

            var loanRecord = await this.loanRecordRepository.GetByIdAsync(foreignWorker.Id );
            

            return Ok(new GetLoanDataResponseDto
            {
                StatusCode = 10000,
                Message = "success",
                UserName = foreignWorker.UserName,
                Amount = loanRecord.Amount,
                Period = loanRecord.Period
            });
        }
/*
            var loanRecord = await this.loanRecordRepository.GetByIdAndStateAsync(foreignWorker.Id, state);
             if(loanRecord.State != 0)
             {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10007,
                    Message = "status not correct"
                }); 
             }
      
            return Ok(new GetLoanDataResponseDto
            {
                StatusCode = 10030,
                Message = "Success",
                UserName = foreignWorker.UserName,
                Amount = loanRecord.Amount,
                Period = loanRecord.Period
            });            
        }

*/

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

        public static string GenerateLoanNumber(string nationality)
        {          
            string number = nationality.Substring(0,1) + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", sn);                                               
            sn ++; 
            return number;
        }
    }
}
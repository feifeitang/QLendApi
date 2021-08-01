using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Extensions;
using QLendApi.Repositories;
using Microsoft.AspNetCore.Mvc;

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

        public LoanController(
            ILoanRecordRepository loanRecordRepository,
            IForeignWorkerRepository foreignWorkerRepository,
            ICertificateRepository certificateRepository)
        {
            this.loanRecordRepository = loanRecordRepository;

            this.foreignWorkerRepository = foreignWorkerRepository;

            this.certificateRepository = certificateRepository;
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
                    Purpose = loanApply.Purpose 
                };
        
                await loanRecordRepository.CreateLoanRecordAsync(loanRecord);                        

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

        

        // POST /api/loan/arc
        [Route("arc")]
        [HttpPost]
        public ActionResult Arc()
        {
            return StatusCode(201);
        }

        // POST /api/loan/signature
        [Route("signature")]
        [HttpPost]
        public ActionResult Signature()
        {
            return StatusCode(201);
        }

        // POST /api/loan/confirm
        [Route("confirm")]
        [HttpPost]
        public ActionResult Confirm()
        {
            return StatusCode(201);
        }

        // POST /api/loan/bankAccount
        [Route("bankAccount")]
        [HttpPost]
        public ActionResult bankAccount()
        {
            return StatusCode(201);
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

            var loanRecord = await this.loanRecordRepository.GetLoanRecordByIdAndStatusAsync(foreignWorker.Id, status);

            return Ok(new GetLoanListResponseDto{
                StatusCode = 10000,
                Message = "success",
                LoanRecord = loanRecord
            });
        }

    }
}
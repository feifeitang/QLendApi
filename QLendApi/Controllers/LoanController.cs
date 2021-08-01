using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLendApi.Dtos;
using QLendApi.lib;
using QLendApi.Models;
using QLendApi.Extensions;
using QLendApi.Repositories;

namespace QLendApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRecordRepository loanRecordRepository;
        private readonly IForeignWorkerRepository foreignWorkerRepository;
        private readonly IIncomeInformationRepository incomeInformationRepository;

        private readonly ICertificateRepository certificateRepository;
        public LoanController(
            ILoanRecordRepository loanRecordRepository,
            IForeignWorkerRepository foreignWorkerRepository,
            IIncomeInformationRepository incomeInformationRepository,
            ICertificateRepository certificateRepository)
        {
            this.loanRecordRepository = loanRecordRepository;
            this.foreignWorkerRepository = foreignWorkerRepository;
            this.incomeInformationRepository = incomeInformationRepository;
            this.certificateRepository = certificateRepository;
        }

        //POST /api/loan/apply
        [Route("apply")]
        [HttpPost]
        public async Task<ActionResult> LoanApply(LoanApplyDto loanApply)
         {     
            // check user if exist
            var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(loanApply.Id);

            if (foreignWorker == null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10003,
                    Message = "user not found"
                });
            }

            LoanRecord loanRecord = new()
            {
                Amount = loanApply.Amount,
                Period = loanApply.Period,
                Purpose = loanApply.Purpose 
            };
        
            await loanRecordRepository.CreateLoanApplyAsync(loanRecord);                        
            return StatusCode(201);
         }     


        //POST /api/loan/personalInfo1
        [Route("personalInfo1")]
        [HttpPost]
         public async Task<ActionResult> PersonalInfo1(PersonalInfo1 personalInfo1)
         {
             // check user if exist
            var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(personalInfo1.Id);

            if (foreignWorker == null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 10003,
                    Message = "user not found"
                });
            }
            
            foreignWorker.Marriage = personalInfo1.Marriage;
            foreignWorker.ImmediateFamilyNumber = personalInfo1.ImmediateFamilyNumber;
            foreignWorker.EducationBackground = personalInfo1.EducationBackground;
            foreignWorker.TimeInTaiwan = personalInfo1.TimeInTaiwan;
             

             await foreignWorkerRepository.CreateForeignWorkerAsync(foreignWorker);
             return StatusCode(201);
         }

        //POST /api/loan/personalInfo2
        [Route("personalInfo2")]
        [HttpPost]
         public async Task<ActionResult> PersonalInfo2 ([FromForm] PersonalInfo2 personalInfo2)
         {                        
            try
            {
                var foreignWorker = await foreignWorkerRepository.GetForeignWorkerByIdAsync(personalInfo2.Id);

                if (foreignWorker == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10003,
                        Message = "user not found"
                    });
                }

                IncomeInformation incomeInformation = new()
                {
                    AvgMonthlyIncome = personalInfo2.AvgMonthlyIncome,
                    LatePay = personalInfo2.LatePay,
                    PayWay = personalInfo2.PayWay,
                    RemittanceWay = personalInfo2.RemittanceWay
                };

                await incomeInformationRepository.CreatePersonalInfo2Async(incomeInformation);
           

                var salarybook = await incomeInformationRepository.GetSalarybookAsync(foreignWorker.Uino);

                if (salarybook == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        StatusCode = 10006,
                        Message = "salarybook not found"
                    });
                }

                salarybook.FrontSalaryPassbook = await personalInfo2.FrontSalaryPassbook.GetBytes();
                salarybook.InsideSalarybook = await personalInfo2.InsideSalarybook.GetBytes();

                await incomeInformationRepository.UpdateSalarybookAsync(salarybook);

                return StatusCode(201);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = 90005,
                    Message = $"personalInfo2 api error:{ex}"
                });
            }
         }
    }
}
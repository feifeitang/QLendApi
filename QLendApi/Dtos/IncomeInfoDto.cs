using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record IncomeInfoDto
    {
        [Required]
        public string LoanNumber { get; init; }
        
        [Required]
        public int AvgMonthlyIncome { get; init; }

        [Required]
        public int LatePay { get; init; }

        [Required]
        public int PayWay { get; init; }

        [Required]
        public int RemittanceWay { get; init; }

        [Required]
        public IFormFile FrontSalaryPassbook { get; init; }
        
        [Required]
        public IFormFile PaySlip { get; init; }

        [Required]
        public DateTime PayDay { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record IncomeInfoDto
    {
        
        [Required]
        public int AvgMonthlyIncome { get; init; }

        [Required]
        public int LatePay { get; init; }

        [Required]
        public int PayWay { get; init; }

        [Required]
        public int RemittanceWay { get; init; }

        public IFormFile FrontSalaryPassbook { get; init; }
        
        public IFormFile InsideSalarybook { get; init; }
    }
}
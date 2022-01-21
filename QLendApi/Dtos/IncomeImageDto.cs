using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record IncomeImageDto
    {
        [Required]
        public string LoanNumber { get; init; }

        [Required]
        public int type { get; init; }
        public IFormFile FrontSalaryPassbook { get; init; }
        public IFormFile PaySlip { get; init; }     
    }
}
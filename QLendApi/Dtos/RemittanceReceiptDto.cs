using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System;

namespace QLendApi.Dtos
{
    public record RemittanceReceiptDto
    {
        [Required]
        public string RepaymentNumber { get; init; }

        [Required]
        [StringLength(5, MinimumLength = 5)]
        public string RemitAccount { get; set; }

        [Required]
        public DateTime? ActualRepaymentDate { get; set; }
    
        [Required]
        public IFormFile Receipt {get; init;}
    }
}
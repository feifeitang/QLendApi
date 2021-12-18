using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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
        public IFormFile Receipt {get; init;}
    }
}
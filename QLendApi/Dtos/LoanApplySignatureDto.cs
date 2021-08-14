using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record LoanApplySignatureDto
    {
        [Required]
        public string LoanNumber { get; init; }

        [Required]
        public IFormFile Signature { get; init; }
    }
}
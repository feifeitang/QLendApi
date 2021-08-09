using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record LoanApplySignatureDto
    {
        [Required]
        public IFormFile Signature { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record SignatureConfirmDto
    {
        [Required]
        public IFormFile Signature2 { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record SignatureDto
    {
        [Required]
        public IFormFile Signature { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record ConfirmDto
    {
        [Required]
        public string LoanNumber { get; init; }    

        [Required]
        public IFormFile Signature2 { get; init; }
    }
}
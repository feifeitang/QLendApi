using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record LoanCancelDto
    {
        [Required]
        public string LoanNumber { get; init; }
    }
}
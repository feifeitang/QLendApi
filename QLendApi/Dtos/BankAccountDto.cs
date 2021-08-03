using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record BankAccountDto
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string BankNumber { get; init; }

        [Required]
        [StringLength(14)]
        public string AccountNumber { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record LoginDto
    {
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string Uino { get; init; }

        [Required]
        public string Password { get; init; }
    }
}

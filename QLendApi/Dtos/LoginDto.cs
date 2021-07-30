using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record LoginDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Password { get; init; }
    }
}

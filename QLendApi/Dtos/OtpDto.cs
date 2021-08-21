using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record OtpDto
    {
        [Required]
        public int Id { get; init; }
    }
}
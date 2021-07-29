using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record SendOtpDto
    {
        [Required]
        public int Id { get; init; }
    }
}
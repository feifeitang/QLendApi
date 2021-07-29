using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record CheckOtpDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public int OTP { get; init; }
    }
}

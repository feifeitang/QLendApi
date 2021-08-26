using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record OtpByPhoneNumberDto
    {
        [Required]
        public string PhoneNumber { get; init; }
    }
}
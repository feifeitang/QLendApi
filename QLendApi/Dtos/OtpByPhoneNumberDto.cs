using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record OtpByPhoneNumberDto
    {
        
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string UINo { get; init; }


        [Required]
        public string PhoneNumber { get; init; }
    }
}
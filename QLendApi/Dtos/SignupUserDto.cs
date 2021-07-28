using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record SignupUserDto
    {
        [Required]
        [StringLength(10, MinimumLength = 10)]

        public string UINo { get; init; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Password { get; init; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; init; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record UserUpdateDto
    {
        [Required]
        public int Marriage { get; init;}

        [Required]
        public int ImmediateFamilyNumber { get; init;}

        [Required]
        public int EducationBackground { get; init;}

        [Required]
        public int TimeInTaiwan { get; init;}

    }
}
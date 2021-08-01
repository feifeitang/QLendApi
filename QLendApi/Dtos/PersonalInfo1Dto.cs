using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record PersonalInfo1
    {
        [Required]
        public int Id { get; init; }
        [Required]
        public int Marriage {get; init;}
        [Required]
        public int ImmediateFamilyNumber {get; init;}
        [Required]
        public int EducationBackground { get; init;}
        [Required]
        public int TimeInTaiwan {get; init;}

    }
}
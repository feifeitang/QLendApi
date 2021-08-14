using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record LoanSurveyInfoDto
    {
        [Required]
        public string LoanNumber { get; init; }

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
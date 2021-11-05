using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record LoanSurveyArcDto
    {
     //   [Required]
      //  public string LoanNumber { get; init; }

        [Required]
        public int Type { get; init; }


     //   [Required]
        public IFormFile FrontArc2 {get; init;}

     //   [Required]
        public IFormFile BackArc2 {get; init;}

      //  [Required]
        public IFormFile SelfieArc {get; init;}
    }
}
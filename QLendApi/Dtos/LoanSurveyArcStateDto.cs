using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record LoanSurveyArcStateDto
    {
        [Required]
        public string LoanNumber { get; init; }
    }
}
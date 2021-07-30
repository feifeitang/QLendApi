using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record ArcDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public IFormFile FrontArc { get; init; }

        [Required]
        public IFormFile BackArc { get; init; }
    }
}
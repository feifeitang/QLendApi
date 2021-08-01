using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record ArcWithSelfieDto
    {
        [Required]
        public IFormFile SelfileArc { get; init; }
        [Required]
        public IFormFile FrontArc2 { get; init; }
        [Required]
        public IFormFile BackArc2 { get; init; }
    }
}
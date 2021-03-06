using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record ImageUploadDto
    {
        [Required]
        public int Id { get; init; }

        // [Required]
        public IFormFile FrontArc { get; init; }

        // [Required]
        public IFormFile BackArc { get; init; }

        // [Required]
        public IFormFile Passport { get; init; }

        // [Required]
        public IFormFile LocalIdCard { get; init; }

        [Required]
        public int Type { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record UpdateArcDto
    {
    
        [Required]
        public IFormFile FrontArc {get; init;}

        [Required]
        public IFormFile BackArc {get; init;}

        [Required]
        public IFormFile SelfieArc {get; init;}

    }
}
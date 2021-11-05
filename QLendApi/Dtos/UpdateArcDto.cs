using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record UpdateArcDto
    {
        [Required]
        public int Type { get; init; }
    
       
        public IFormFile FrontArc {get; init;}

        public IFormFile BackArc {get; init;}

    
        public IFormFile SelfieArc {get; init;}

    }
}
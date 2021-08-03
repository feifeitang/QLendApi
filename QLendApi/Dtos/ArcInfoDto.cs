using System;
using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record ArcInfoDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public DateTime DateOfIssue { get; init; }
        
        [Required]
        public DateTime DateOfExpiry { get; init; }
        
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string BarcodeNumber { get; init; }

        [Required]
        public int? KindOfWork { get; init; }

        [Required]
        public int? Workplace { get; init; }
    }
}
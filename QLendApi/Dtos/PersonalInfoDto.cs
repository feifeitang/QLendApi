using System;
using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record PersonalInfoDto
    {
        [Required]
        public int Id { get; init; }

        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string UserName { get; init; }
        
        [Required]
        [StringLength(20)]
        public string EnglishName { get; init; }
        
        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Gender { get; init; }
        
        [Required]
        [StringLength(11, MinimumLength = 7)]
        public string Nationality { get; init; }
        
        [Required]
        public DateTime DateOfIssue { get; init; }
        
        [Required]
        public DateTime DateOfExpiry { get; init; }
        
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string BarcodeNumber { get; init; }
    }
}
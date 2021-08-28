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
        public string Sex { get; init; }
        
        [Required]
        [StringLength(11, MinimumLength = 7)]
        public string Nationality { get; init; }

        [Required]
        public DateTime? BirthDate { get; init; }

        [Required]
        public string FamilyMemberName { get; set; }
        
        [Required]
        public string FamilyMemberPhoneNumber { get; set; }

        [Required]
        public string FacebookAccount { get; set; }

        [Required]
        public string CommunicationSoftware { get; set; }

        [Required]
        public string CommunicationSoftwareAccount { get; set; }        
    }
}
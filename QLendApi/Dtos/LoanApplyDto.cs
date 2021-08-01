using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record LoanApplyDto
    {
        [Required]
        public int Amount {get; init;}

        [Required]
        public int Period {get; init;}
        
        [Required]
        public int Purpose {get; init;}
    }
}
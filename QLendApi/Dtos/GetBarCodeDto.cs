using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public record GetBarCodeDto
    {
        [Required]
        public string RepaymentRecord { get; init; }
    }
}
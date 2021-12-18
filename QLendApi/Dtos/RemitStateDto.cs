using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record RemitStateDto
    {
        [Required]
        public string RepaymentNumber { get; init; }
    }
}
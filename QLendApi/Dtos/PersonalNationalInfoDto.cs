using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QLendApi.Dtos
{
    public record PersonalNationalInfoDto
    {
        [Required]
        public int Id { get; init; }
    }
}
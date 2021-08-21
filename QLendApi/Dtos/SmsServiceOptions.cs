using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class SmsServiceOptions
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

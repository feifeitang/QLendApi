using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}
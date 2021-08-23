using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class EcpayServiceOptions
    {
        [Required]
        public string HashIV { get; set; }
        [Required]
        public string HashKey { get; set; }
        [Required]
        public string MerchantID { get; set; }
    }
}

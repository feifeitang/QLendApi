using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class EcpayCreateOrderDto
    {
        [Required]
        public string MerchantID { get; set; }
        [Required]
        [StringLength(20)]
        public string MerchantTradeNo { get; set; }
        [Required]
        public string MerchantTradeDate { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public int TotalAmount { get; set; }
        [Required]
        public int StoreExpireDate { get; set; }
        [Required]
        public string TradeDesc { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string ReturnURL { get; set; }
        [Required]
        public string ChoosePayment { get; set; }
        [Required]
        public string CheckMacValue { get; set; }
        [Required]
        public int EncryptType { get; set; }
        [Required]
        public string PaymentInfoURL { get; set; }
    }
}

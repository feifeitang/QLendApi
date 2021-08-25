using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class EcpayReceivePaymentInfoDto
    {
        // [Required]
        public string MerchantID { get; set; }
        public string PlatformID { get; set; }
        // [Required]
        public string MerchantTradeNo { get; set; }
        // [Required]
        public string TradeNo { get; set; }
        // [Required]
        public string MerchantTradeDate { get; set; }
        // [Required]
        public string PaymentType { get; set; }
        // [Required]
        public int RtnCode { get; set; }
        // [Required]
        public string RtnMsg { get; set; }
        // [Required]
        public int TradeAmt { get; set; }
        // [Required]
        public string TradeDate { get; set; }
        // [Required]
        public string CheckMacValue { get; set; }
        // [Required]
        public string ExpireDate { get; set; }
        // [Required]
        public string Barcode1 { get; set; }
        // [Required]
        public string Barcode2 { get; set; }
        // [Required]
        public string Barcode3 { get; set; }
    }
}

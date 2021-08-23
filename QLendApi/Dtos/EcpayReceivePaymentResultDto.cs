using System.ComponentModel.DataAnnotations;

namespace QLendApi.Dtos
{
    public class EcpayReceivePaymentResultDto
    {
        [Required]
        public string MerchantID { get; set; }
        public string PlatformID { get; set; }
        [Required]
        [StringLength(20)]
        public string MerchantTradeNo { get; set; }
        [Required]
        public string TradeNo { get; set; }
        [Required]
        public string PaymentDate { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public int PaymentTypeChargeFee { get; set; }
        [Required]
        public int SimulatePaid { get; set; }
        [Required]
        public int RtnCode { get; set; }
        [Required]
        public string RtnMsg { get; set; }
        [Required]
        public int TradeAmt { get; set; }
        [Required]
        public string TradeDate { get; set; }
        [Required]
        public string CheckMacValue { get; set; }
    }
}

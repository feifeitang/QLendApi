using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace QLendApi.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "char(20)")]
        public string MerchantTradeNo { get; set; }
        public int Amount { get; set; }
        public string ReturnURL { get; set; }
        public string PaymentInfoURL { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode1 { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode2 { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode3 { get; set; }
        [Column(TypeName = "char(20)")]
        public string TradeNo { get; set; }
        public int Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? ReceivePayTime { get; set; }
        [Column(TypeName = "char(11)")]
        public string RepaymentNumber { get; set; }
    }
}

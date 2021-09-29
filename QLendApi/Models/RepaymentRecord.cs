using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace QLendApi.Models
{
    public partial class RepaymentRecord
    {
        public string RepaymentNumber { get; set; }
        public int RepaymentOrder { get; set; }
        public DateTime RepaymentDate { get; set; }
        public DateTime? ActualRepaymentDate { get; set; }
        public int RepaymentAmount { get; set; }
        public int? ActualRepaymentAmount { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode1 { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode2 { get; set; }
        [Column(TypeName = "char(20)")]
        public string BarCode3 { get; set; }
        public int State { get; set; }
        public string LoanNumber { get; set; }
        public DateTime? BarCodeCreateTime { get; set; }

        public virtual LoanRecord LoanNumberNavigation { get; set; }
    }
}

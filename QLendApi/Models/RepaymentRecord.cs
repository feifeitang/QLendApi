using System;
using System.Collections.Generic;

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
        public byte[] RepaymentBarCode { get; set; }
        public int State { get; set; }
        public string LoanNumber { get; set; }

        public virtual LoanRecord LoanNumberNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace QLendApi.Models
{
    public partial class LoanRecord
    {
        public LoanRecord()
        {
            RepaymentRecords = new HashSet<RepaymentRecord>();
        }

        public string LoanNumber { get; set; }
        public DateTime? LoanDate { get; set; }
        public int Amount { get; set; }
        public int Period { get; set; }
        public double? InterestRate { get; set; }
        public int Purpose { get; set; }
        public int? Status { get; set; }
        public int? State { get; set; }
        public int? Auditor { get; set; }
        public int? Id { get; set; }
        public string Idno { get; set; }

        public virtual ForeignWorker IdNavigation { get; set; }
        public virtual CreditSide IdnoNavigation { get; set; }
        public virtual ICollection<RepaymentRecord> RepaymentRecords { get; set; }
    }
}

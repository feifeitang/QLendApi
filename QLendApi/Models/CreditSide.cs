using System;
using System.Collections.Generic;

#nullable disable

namespace QLendApi.Models
{
    public partial class CreditSide
    {
        public CreditSide()
        {
            LoanRecords = new HashSet<LoanRecord>();
        }

        public string Idno { get; set; }
        public string CreditName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CreditPassword { get; set; }
        public string BankNumber { get; set; }
        public string AccountNumber { get; set; }
        public byte[] CreditSignature { get; set; }
        public byte[] CreditSignature2 { get; set; }

        public virtual ICollection<LoanRecord> LoanRecords { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace QLendApi.Models
{
    public partial class LoanRecord
    {
        public LoanRecord()
        {
            RepaymentRecords = new HashSet<RepaymentRecord>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public DateTime CreateTime { get; set; }

        [JsonIgnore]
        public virtual ForeignWorker IdNavigation { get; set; }
        [JsonIgnore]
        public virtual CreditSide IdnoNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<RepaymentRecord> RepaymentRecords { get; set; }
    }
}

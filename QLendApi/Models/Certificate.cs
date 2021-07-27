using System;
using System.Collections.Generic;

#nullable disable

namespace QLendApi.Models
{
    public partial class Certificate
    {
        public Certificate()
        {
            ForeignWorkers = new HashSet<ForeignWorker>();
        }

        public string Uino { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ResidencePurpose { get; set; }
        public string BarcodeNumber { get; set; }
        public byte[] FrontArc { get; set; }
        public byte[] BackArc { get; set; }
        public byte[] SelfileArc { get; set; }
        public byte[] FrontArc2 { get; set; }
        public byte[] BackArc2 { get; set; }

        public virtual ICollection<ForeignWorker> ForeignWorkers { get; set; }
    }
}

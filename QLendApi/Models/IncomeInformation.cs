using System;
using System.Collections.Generic;

#nullable disable

namespace QLendApi.Models
{
    public partial class IncomeInformation
    {
        public IncomeInformation()
        {
            ForeignWorkers = new HashSet<ForeignWorker>();
        }

        public int IncomeNumber { get; set; }
        public int AvgMonthlyIncome { get; set; }
        public int LatePay { get; set; }
        public int PayWay { get; set; }
        public int RemittanceWay { get; set; }
        public byte[] FrontSalaryPassbook { get; set; }
        public byte[] InsideSalarybook { get; set; }

        public virtual ICollection<ForeignWorker> ForeignWorkers { get; set; }
    }
}

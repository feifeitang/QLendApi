using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace QLendApi.Models
{
    public partial class Employer
    {
        public Employer()
        {
            ForeignWorkers = new HashSet<ForeignWorker>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployerNo { get; set; }
        public string Name { get; set; }
        public bool? ListedOrNot { get; set; }
        public bool? CapitalMoreThan30m { get; set; }
        public string CompanyCode { get; set; }

        public virtual CompanyList CompanyCodeNavigation { get; set; }
        public virtual ICollection<ForeignWorker> ForeignWorkers { get; set; }
    }
}

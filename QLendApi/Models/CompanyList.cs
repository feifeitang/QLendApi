using System;
using System.Collections.Generic;

#nullable disable

namespace QLendApi.Models
{
    public partial class CompanyList
    {
        public CompanyList()
        {
            Employers = new HashSet<Employer>();
        }

        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

        public virtual ICollection<Employer> Employers { get; set; }
    }
}

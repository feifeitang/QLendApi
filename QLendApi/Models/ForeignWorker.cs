using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace QLendApi.Models
{
    public partial class ForeignWorker
    {
        public ForeignWorker()
        {
            LoanRecords = new HashSet<LoanRecord>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        [Column(TypeName = "char(60)")]

        [JsonIgnore]
        public string Password { get; set; }
        public int? Marriage { get; set; }
        public int? ImmediateFamilyNumber { get; set; }
        public int? EducationBackground { get; set; }
        public int? TimeInTaiwan { get; set; }
        public string UserName { get; set; }
        public string PassportNumber { get; set; }
        public byte[] Signature { get; set; }
        public int? CreditScore { get; set; }
        public int? DefaultRisk { get; set; }
        public string BankNumber { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? RegisterTime { get; set; }
        public int Status { get; set; }
        public int? OTP { get; set; }
        public DateTime? OTPSendTIme { get; set; }
        public int? State { get; set; }
        public int? KindOfWork { get; set; }
        public int? Workplace { get; set; }
        public string Uino { get; set; }
        [Column(TypeName = "varchar(32)")]
        public string DeviceTag { get; set; }
        public int? IncomeNumber { get; set; }
        public int? EmployerNumber { get; set; }
        public byte[] Signature2 { get; set; }

        public virtual Employer EmployerNumberNavigation { get; set; }
        public virtual IncomeInformation IncomeNumberNavigation { get; set; }
        public virtual Certificate UinoNavigation { get; set; }
        public virtual ICollection<LoanRecord> LoanRecords { get; set; }
    }
}

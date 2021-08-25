using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace QLendApi.Models
{
    public partial class QLendDBContext : DbContext
    {
        public QLendDBContext()
        {
        }

        public QLendDBContext(DbContextOptions<QLendDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<CompanyList> CompanyLists { get; set; }
        public virtual DbSet<CreditSide> CreditSides { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<ForeignWorker> ForeignWorkers { get; set; }
        public virtual DbSet<IncomeInformation> IncomeInformations { get; set; }
        public virtual DbSet<LoanRecord> LoanRecords { get; set; }
        public virtual DbSet<RepaymentRecord> RepaymentRecords { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DatabaseAlias");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasKey(e => e.Uino)
                    .HasName("PK__Certific__B1FE46809DDC58EA");

                entity.ToTable("Certificate");

                entity.HasComment("居留證");

                entity.Property(e => e.Uino)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UINo")
                    .IsFixedLength(true)
                    .HasComment("統一證號");

                entity.Property(e => e.BackArc)
                    .HasColumnType("image")
                    .HasColumnName("BackARC")
                    .HasComment("居留證反面照片");

                entity.Property(e => e.BackArc2)
                    .HasColumnType("image")
                    .HasColumnName("BackARC2");

                entity.Property(e => e.BarcodeNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("居留證流水號");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("date")
                    .HasComment("居留期限");

                entity.Property(e => e.FrontArc)
                    .HasColumnType("image")
                    .HasColumnName("FrontARC")
                    .HasComment("居留證正面照片");

                entity.Property(e => e.FrontArc2)
                    .HasColumnType("image")
                    .HasColumnName("FrontARC2");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("date")
                    .HasComment("核發日期");

                entity.Property(e => e.ResidencePurpose)
                    .HasMaxLength(70)
                    .IsFixedLength(true)
                    .HasComment("居留事由，由中台填");

                entity.Property(e => e.SelfileArc)
                    .HasColumnType("image")
                    .HasColumnName("SelfileARC")
                    .HasComment("自己與居留證合照照片");
            });

            modelBuilder.Entity<CompanyList>(entity =>
            {
                entity.HasKey(e => e.CompanyCode)
                    .HasName("PK__CompanyL__11A0134A2607838A");

                entity.ToTable("CompanyList");

                entity.HasComment("公司名單");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("上市櫃代碼");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength(true)
                    .HasComment("公司名稱");
            });

            modelBuilder.Entity<CreditSide>(entity =>
            {
                entity.HasKey(e => e.Idno)
                    .HasName("PK__CreditSi__B87DC9AB36CAC589");

                entity.ToTable("CreditSide");

                entity.HasComment("貸方");

                entity.Property(e => e.Idno)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDNo")
                    .IsFixedLength(true)
                    .HasComment("身分證");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("銀行帳號");

                entity.Property(e => e.BankNumber)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("銀行代碼");

                entity.Property(e => e.CreditName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("姓名");

                entity.Property(e => e.CreditPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("密碼");

                entity.Property(e => e.CreditSignature)
                    .IsRequired()
                    .HasColumnType("image")
                    .HasComment("手寫簽名");

                entity.Property(e => e.CreditSignature2).HasColumnType("image");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("電子郵件");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("手機號碼");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("使用者名稱");
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.HasKey(e => e.EmployerNo)
                    .HasName("PK__Employer__CA446ACE1D7D0603");

                entity.ToTable("Employer");

                entity.HasComment("雇主/公司");

                entity.Property(e => e.EmployerNo).HasComment("雇主編號");

                entity.Property(e => e.CapitalMoreThan30m).HasComment("是否資本額3000萬以上");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("上市櫃代碼");

                entity.Property(e => e.ListedOrNot).HasComment("是否上市櫃");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength(true)
                    .HasComment("雇主名字/公司名");

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.Employers)
                    .HasForeignKey(d => d.CompanyCode)
                    .HasConstraintName("FK__Employer__Compan__3D5E1FD2");
            });

            modelBuilder.Entity<ForeignWorker>(entity =>
            {
                entity.ToTable("ForeignWorker");

                entity.HasComment("外勞");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("外勞會員流水號");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("銀行帳號");

                entity.Property(e => e.BankNumber)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("銀行代碼");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasComment("出生日期");

                entity.Property(e => e.CreditScore).HasComment("信評分數 1~5");

                entity.Property(e => e.DefaultRisk).HasComment("違約風險 1~10");

                entity.Property(e => e.EducationBackground).HasComment("最高學歷 1:國小畢業 2:國中畢業 3:高中職(含)以上");

                entity.Property(e => e.EmployerNumber).HasComment("雇主編號");

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("英文名字");

                entity.Property(e => e.ImmediateFamilyNumber).HasComment("直系親屬數");

                entity.Property(e => e.IncomeNumber).HasComment("收入流水編號");

                entity.Property(e => e.KindOfWork).HasComment("工作性質 1:社福移工 2:產業移工 3:其他");

                entity.Property(e => e.Marriage).HasComment("婚姻狀態 1:單身 2:已婚");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength(true)
                    .HasComment("中文名字");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("國籍");
/*
                entity.Property(e => e.PassportNumber)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("護照號碼");
*/
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("密碼 最少8碼 英文和數字(英文不用限制大小寫)");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("手機號碼");

                entity.Property(e => e.RegisterTime)
                    .HasColumnType("date")
                    .HasComment("會員註冊審核狀態 0:審核中 1:審核通過");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("性別 M:男生 F:女生");

                entity.Property(e => e.Signature)
                    .HasColumnType("image")
                    .HasComment("手寫簽名");

                entity.Property(e => e.Signature2).HasColumnType("image");

                entity.Property(e => e.State).HasComment("會員註冊時間 有更新(資料建立、審查通過/沒過)都改這個時間");

                entity.Property(e => e.TimeInTaiwan).HasComment("來台時長 1:未滿1年 2:1~未滿3年 3:9~12年 4:3~9年");

                entity.Property(e => e.Uino)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UINo")
                    .IsFixedLength(true)
                    .HasComment("統一證號");

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("使用者名稱");

                entity.Property(e => e.Workplace).HasComment("工作地點 1:基隆市 2:台北市 3:新北市 4:桃園市 5:新竹市 6:新竹縣 7:苗栗縣 8:南投縣 9:台中市 10:彰化縣 11:雲林縣 12:嘉義縣 13:嘉義市 14:台南市 15:高雄市 16:屏東縣 17:宜蘭縣 18:花蓮縣 19:台東縣 20:澎湖縣");

                entity.HasOne(d => d.EmployerNumberNavigation)
                    .WithMany(p => p.ForeignWorkers)
                    .HasForeignKey(d => d.EmployerNumber)
                    .HasConstraintName("FK__ForeignWo__Emplo__4D94879B");

                entity.HasOne(d => d.IncomeNumberNavigation)
                    .WithMany(p => p.ForeignWorkers)
                    .HasForeignKey(d => d.IncomeNumber)
                    .HasConstraintName("FK__ForeignWo__Incom__4CA06362");

                entity.HasOne(d => d.UinoNavigation)
                    .WithMany(p => p.ForeignWorkers)
                    .HasForeignKey(d => d.Uino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ForeignWo__Emplo__4BAC3F29");
            });

            modelBuilder.Entity<IncomeInformation>(entity =>
            {
                entity.HasKey(e => e.IncomeNumber)
                    .HasName("PK__IncomeIn__A42D1F407AC301EC");

                entity.ToTable("IncomeInformation");

                entity.HasComment("收入資訊");

                entity.Property(e => e.IncomeNumber).HasComment("收入資訊流水編號");

                entity.Property(e => e.AvgMonthlyIncome).HasComment("平均月收入 1:17000~24000 2:24000~3000 3:30000以上");

                entity.Property(e => e.FrontSalaryPassbook)
                    .HasColumnType("image")
                    .HasComment("薪資存摺本正面照片");

                entity.Property(e => e.InsideSalarybook)
                    .HasColumnType("image")
                    .HasComment("薪資存摺本內部照片");

                entity.Property(e => e.LatePay).HasComment("薪資如期撥入 1:3次以上 2:1~2次 3:從未延發");

                entity.Property(e => e.PayWay).HasComment("薪資撥付方式 0:現金領取 1:銀行入賬");

                entity.Property(e => e.RemittanceWay).HasComment("薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款");
            });

            modelBuilder.Entity<LoanRecord>(entity =>
            {
                entity.HasKey(e => e.LoanNumber)
                    .HasName("PK__LoanReco__EEC2662937B8C5C8");

                entity.ToTable("LoanRecord");

                entity.HasComment("借貸紀錄");

                entity.Property(e => e.LoanNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("借貸編號");

                entity.Property(e => e.Amount).HasComment("金額");

                entity.Property(e => e.Auditor).HasComment("審核者 0:系統 1:人工");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("外勞會員流水號");

                entity.Property(e => e.Idno)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IDNo")
                    .IsFixedLength(true)
                    .HasComment("身分證(貸方)");

                entity.Property(e => e.InterestRate).HasComment("利率");

                entity.Property(e => e.LoanDate)
                    .HasColumnType("date")
                    .HasComment("借款日期");

                entity.Property(e => e.Period).HasComment("期數");

                entity.Property(e => e.Purpose).HasComment("用途 1:其他 2:投資理財 3:個人消費(食衣住行) 4:家鄉急用");

                entity.Property(e => e.State).HasComment("借款狀態 0:審核中 1:允許借款 2:確認借款 3:已匯款 -1:拒絕借款");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.LoanRecords)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__LoanRecord__ID__5070F446");

                entity.HasOne(d => d.IdnoNavigation)
                    .WithMany(p => p.LoanRecords)
                    .HasForeignKey(d => d.Idno)
                    .HasConstraintName("FK__LoanRecord__IDNo__5165187F");
            });

            modelBuilder.Entity<RepaymentRecord>(entity =>
            {
                entity.HasKey(e => e.RepaymentNumber)
                    .HasName("PK__Repaymen__80FF429F3AC78ED1");

                entity.ToTable("RepaymentRecord");

                entity.HasComment("還款紀錄");

                entity.Property(e => e.RepaymentNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("還款編號");

                entity.Property(e => e.ActualRepaymentAmount).HasComment("實際還款金額");

                entity.Property(e => e.ActualRepaymentDate)
                    .HasColumnType("date")
                    .HasComment("實際還款日");

                entity.Property(e => e.LoanNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasComment("借貸編號");

                entity.Property(e => e.RepaymentAmount).HasComment("還款金額");


                entity.Property(e => e.RepaymentDate)
                    .HasColumnType("date")
                    .HasComment("還款日期");

                entity.Property(e => e.RepaymentOrder).HasComment("還款序號");

                entity.Property(e => e.State).HasComment("還款狀態 -1:expired 0:unpaid 1:paid");

                entity.HasOne(d => d.LoanNumberNavigation)
                    .WithMany(p => p.RepaymentRecords)
                    .HasForeignKey(d => d.LoanNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Repayment__LoanN__5441852A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

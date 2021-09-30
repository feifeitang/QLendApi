﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QLendApi.Models;

namespace QLendApi.Migrations
{
    [DbContext(typeof(QLendDBContext))]
    [Migration("20210929171640_deleteLimit")]
    partial class deleteLimit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("QLendApi.Models.Certificate", b =>
                {
                    b.Property<string>("Uino")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("UINo")
                        .IsFixedLength(true)
                        .HasComment("統一證號");

                    b.Property<byte[]>("BackArc")
                        .HasColumnType("image")
                        .HasColumnName("BackARC")
                        .HasComment("居留證反面照片");

                    b.Property<byte[]>("BackArc2")
                        .HasColumnType("image")
                        .HasColumnName("BackARC2");

                    b.Property<string>("BarcodeNumber")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength(true)
                        .HasComment("居留證流水號");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("date")
                        .HasComment("居留期限");

                    b.Property<byte[]>("FrontArc")
                        .HasColumnType("image")
                        .HasColumnName("FrontARC")
                        .HasComment("居留證正面照片");

                    b.Property<byte[]>("FrontArc2")
                        .HasColumnType("image")
                        .HasColumnName("FrontARC2");

                    b.Property<DateTime?>("IssueDate")
                        .HasColumnType("date")
                        .HasComment("核發日期");

                    b.Property<string>("ResidencePurpose")
                        .HasMaxLength(70)
                        .HasColumnType("nchar(70)")
                        .IsFixedLength(true)
                        .HasComment("居留事由，由中台填");

                    b.Property<byte[]>("SelfileArc")
                        .HasColumnType("image")
                        .HasColumnName("SelfileARC")
                        .HasComment("自己與居留證合照照片");

                    b.HasKey("Uino")
                        .HasName("PK__Certific__B1FE46809DDC58EA");

                    b.ToTable("Certificate");

                    b
                        .HasComment("居留證");
                });

            modelBuilder.Entity("QLendApi.Models.CompanyList", b =>
                {
                    b.Property<string>("CompanyCode")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength(true)
                        .HasComment("上市櫃代碼");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nchar(100)")
                        .IsFixedLength(true)
                        .HasComment("公司名稱");

                    b.HasKey("CompanyCode")
                        .HasName("PK__CompanyL__11A0134A2607838A");

                    b.ToTable("CompanyList");

                    b
                        .HasComment("公司名單");
                });

            modelBuilder.Entity("QLendApi.Models.CreditSide", b =>
                {
                    b.Property<string>("Idno")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("IDNo")
                        .IsFixedLength(true)
                        .HasComment("身分證");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(14)
                        .IsUnicode(false)
                        .HasColumnType("char(14)")
                        .IsFixedLength(true)
                        .HasComment("銀行帳號");

                    b.Property<string>("BankNumber")
                        .IsRequired()
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .IsFixedLength(true)
                        .HasComment("銀行代碼");

                    b.Property<string>("CreditName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength(true)
                        .HasComment("姓名");

                    b.Property<string>("CreditPassword")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength(true)
                        .HasComment("密碼");

                    b.Property<byte[]>("CreditSignature")
                        .IsRequired()
                        .HasColumnType("image")
                        .HasComment("手寫簽名");

                    b.Property<byte[]>("CreditSignature2")
                        .HasColumnType("image");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("char(50)")
                        .IsFixedLength(true)
                        .HasComment("電子郵件");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength(true)
                        .HasComment("手機號碼");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength(true)
                        .HasComment("使用者名稱");

                    b.HasKey("Idno")
                        .HasName("PK__CreditSi__B87DC9AB36CAC589");

                    b.ToTable("CreditSide");

                    b
                        .HasComment("貸方");
                });

            modelBuilder.Entity("QLendApi.Models.Employer", b =>
                {
                    b.Property<int>("EmployerNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("雇主編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("CapitalMoreThan30m")
                        .HasColumnType("bit")
                        .HasComment("是否資本額3000萬以上");

                    b.Property<string>("CompanyCode")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength(true)
                        .HasComment("上市櫃代碼");

                    b.Property<bool?>("ListedOrNot")
                        .HasColumnType("bit")
                        .HasComment("是否上市櫃");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nchar(100)")
                        .IsFixedLength(true)
                        .HasComment("雇主名字/公司名");

                    b.HasKey("EmployerNo")
                        .HasName("PK__Employer__CA446ACE1D7D0603");

                    b.HasIndex("CompanyCode");

                    b.ToTable("Employer");

                    b
                        .HasComment("雇主/公司");
                });

            modelBuilder.Entity("QLendApi.Models.ForeignWorker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasComment("外勞會員流水號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasMaxLength(14)
                        .IsUnicode(false)
                        .HasColumnType("char(14)")
                        .IsFixedLength(true)
                        .HasComment("銀行帳號");

                    b.Property<string>("BankNumber")
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .IsFixedLength(true)
                        .HasComment("銀行代碼");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("date")
                        .HasComment("出生日期");

                    b.Property<string>("CommunicationSoftware")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommunicationSoftwareAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CreditScore")
                        .HasColumnType("int")
                        .HasComment("信評分數 1~5");

                    b.Property<int?>("DefaultRisk")
                        .HasColumnType("int")
                        .HasComment("違約風險 1~10");

                    b.Property<string>("DeviceTag")
                        .HasColumnType("varchar(32)");

                    b.Property<int?>("EducationBackground")
                        .HasColumnType("int")
                        .HasComment("最高學歷 1:國小畢業 2:國中畢業 3:高中職(含)以上");

                    b.Property<int?>("EmployerNumber")
                        .HasColumnType("int")
                        .HasComment("雇主編號");

                    b.Property<string>("EnglishName")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength(true)
                        .HasComment("英文名字");

                    b.Property<string>("FacebookAccount")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyMemberName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FamilyMemberPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ImmediateFamilyNumber")
                        .HasColumnType("int")
                        .HasComment("直系親屬數");

                    b.Property<int?>("IncomeNumber")
                        .HasColumnType("int")
                        .HasComment("收入流水編號");

                    b.Property<int?>("KindOfWork")
                        .HasColumnType("int")
                        .HasComment("工作性質 1:社福移工 2:產業移工 3:其他");

                    b.Property<byte[]>("LocalIdCard")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("Marriage")
                        .HasColumnType("int")
                        .HasComment("婚姻狀態 1:單身 2:已婚");

                    b.Property<string>("Name")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength(true)
                        .HasComment("中文名字");

                    b.Property<string>("Nationality")
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .IsFixedLength(true)
                        .HasComment("國籍");

                    b.Property<int?>("OTP")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OTPSendTIme")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Passport")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PassportNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength(true)
                        .HasComment("密碼 最少8碼 英文和數字(英文不用限制大小寫)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength(true)
                        .HasComment("手機號碼");

                    b.Property<DateTime?>("RegisterTime")
                        .HasColumnType("date")
                        .HasComment("會員註冊審核狀態 0:審核中 1:審核通過");

                    b.Property<string>("Sex")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("char(1)")
                        .IsFixedLength(true)
                        .HasComment("性別 M:男生 F:女生");

                    b.Property<byte[]>("Signature")
                        .HasColumnType("image")
                        .HasComment("手寫簽名");

                    b.Property<byte[]>("Signature2")
                        .HasColumnType("image");

                    b.Property<int?>("State")
                        .HasColumnType("int")
                        .HasComment("會員註冊時間 有更新(資料建立、審查通過/沒過)都改這個時間");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TimeInTaiwan")
                        .HasColumnType("int")
                        .HasComment("來台時長 1:未滿1年 2:1~未滿3年 3:9~12年 4:3~9年");

                    b.Property<string>("Uino")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("UINo")
                        .IsFixedLength(true)
                        .HasComment("統一證號");

                    b.Property<string>("UserName")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("char(20)")
                        .IsFixedLength(true)
                        .HasComment("使用者名稱");

                    b.Property<int?>("Workplace")
                        .HasColumnType("int")
                        .HasComment("工作地點 1:基隆市 2:台北市 3:新北市 4:桃園市 5:新竹市 6:新竹縣 7:苗栗縣 8:南投縣 9:台中市 10:彰化縣 11:雲林縣 12:嘉義縣 13:嘉義市 14:台南市 15:高雄市 16:屏東縣 17:宜蘭縣 18:花蓮縣 19:台東縣 20:澎湖縣");

                    b.HasKey("Id");

                    b.HasIndex("EmployerNumber");

                    b.HasIndex("IncomeNumber");

                    b.HasIndex("Uino");

                    b.ToTable("ForeignWorker");

                    b
                        .HasComment("外勞");
                });

            modelBuilder.Entity("QLendApi.Models.IncomeInformation", b =>
                {
                    b.Property<int>("IncomeNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("收入資訊流水編號")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AvgMonthlyIncome")
                        .HasColumnType("int")
                        .HasComment("平均月收入 1:17000~24000 2:24000~3000 3:30000以上");

                    b.Property<byte[]>("FrontSalaryPassbook")
                        .HasColumnType("image")
                        .HasComment("薪資存摺本正面照片");

                    b.Property<int>("LatePay")
                        .HasColumnType("int")
                        .HasComment("薪資如期撥入 1:3次以上 2:1~2次 3:從未延發");

                    b.Property<int>("PayDay")
                        .HasColumnType("int");

                    b.Property<byte[]>("PaySlip")
                        .HasColumnType("image")
                        .HasComment("薪資條");

                    b.Property<int>("PayWay")
                        .HasColumnType("int")
                        .HasComment("薪資撥付方式 0:現金領取 1:銀行入賬");

                    b.Property<int>("RemittanceWay")
                        .HasColumnType("int")
                        .HasComment("薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款");

                    b.HasKey("IncomeNumber")
                        .HasName("PK__IncomeIn__A42D1F407AC301EC");

                    b.ToTable("IncomeInformation");

                    b
                        .HasComment("收入資訊");
                });

            modelBuilder.Entity("QLendApi.Models.LoanRecord", b =>
                {
                    b.Property<string>("LoanNumber")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength(true)
                        .HasComment("借貸編號");

                    b.Property<int>("Amount")
                        .HasColumnType("int")
                        .HasComment("金額");

                    b.Property<int?>("Auditor")
                        .HasColumnType("int")
                        .HasComment("審核者 0:系統 1:人工");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID")
                        .HasComment("外勞會員流水號");

                    b.Property<string>("Idno")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("IDNo")
                        .IsFixedLength(true)
                        .HasComment("身分證(貸方)");

                    b.Property<double?>("InterestRate")
                        .HasColumnType("float")
                        .HasComment("利率");

                    b.Property<DateTime?>("LoanDate")
                        .HasColumnType("date")
                        .HasComment("借款日期");

                    b.Property<int>("Period")
                        .HasColumnType("int")
                        .HasComment("期數");

                    b.Property<int>("Purpose")
                        .HasColumnType("int")
                        .HasComment("用途 1:其他 2:投資理財 3:個人消費(食衣住行) 4:家鄉急用");

                    b.Property<int?>("State")
                        .HasColumnType("int")
                        .HasComment("借款狀態 0:審核中 1:允許借款 2:確認借款 3:已匯款 -1:拒絕借款");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("LoanNumber")
                        .HasName("PK__LoanReco__EEC2662937B8C5C8");

                    b.HasIndex("Id");

                    b.HasIndex("Idno");

                    b.ToTable("LoanRecord");

                    b
                        .HasComment("借貸紀錄");
                });

            modelBuilder.Entity("QLendApi.Models.Notice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ForeignWorkerId")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Notice");
                });

            modelBuilder.Entity("QLendApi.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("BarCode1")
                        .HasColumnType("char(20)");

                    b.Property<string>("BarCode2")
                        .HasColumnType("char(20)");

                    b.Property<string>("BarCode3")
                        .HasColumnType("char(20)");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MerchantTradeNo")
                        .HasColumnType("char(20)");

                    b.Property<string>("PaymentInfoURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReceivePayTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RepaymentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReturnURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TradeNo")
                        .HasColumnType("char(20)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("QLendApi.Models.RepaymentRecord", b =>
                {
                    b.Property<string>("RepaymentNumber")
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .IsFixedLength(true)
                        .HasComment("還款編號");

                    b.Property<int?>("ActualRepaymentAmount")
                        .HasColumnType("int")
                        .HasComment("實際還款金額");

                    b.Property<DateTime?>("ActualRepaymentDate")
                        .HasColumnType("date")
                        .HasComment("實際還款日");

                    b.Property<string>("BarCode1")
                        .HasColumnType("char(20)");

                    b.Property<string>("BarCode2")
                        .HasColumnType("char(20)");

                    b.Property<string>("BarCode3")
                        .HasColumnType("char(20)");

                    b.Property<DateTime?>("BarCodeCreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LoanNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .IsFixedLength(true)
                        .HasComment("借貸編號");

                    b.Property<int>("OriginalAmount")
                        .HasColumnType("int");

                    b.Property<int>("RepaymentAmount")
                        .HasColumnType("int")
                        .HasComment("還款金額");

                    b.Property<DateTime>("RepaymentDate")
                        .HasColumnType("date")
                        .HasComment("還款日期");

                    b.Property<int>("RepaymentOrder")
                        .HasColumnType("int")
                        .HasComment("還款序號");

                    b.Property<int>("State")
                        .HasColumnType("int")
                        .HasComment("還款狀態 -1:expired 0:unpaid 1:paid");

                    b.HasKey("RepaymentNumber")
                        .HasName("PK__Repaymen__80FF429F3AC78ED1");

                    b.HasIndex("LoanNumber");

                    b.ToTable("RepaymentRecord");

                    b
                        .HasComment("還款紀錄");
                });

            modelBuilder.Entity("QLendApi.Models.Employer", b =>
                {
                    b.HasOne("QLendApi.Models.CompanyList", "CompanyCodeNavigation")
                        .WithMany("Employers")
                        .HasForeignKey("CompanyCode")
                        .HasConstraintName("FK__Employer__Compan__3D5E1FD2");

                    b.Navigation("CompanyCodeNavigation");
                });

            modelBuilder.Entity("QLendApi.Models.ForeignWorker", b =>
                {
                    b.HasOne("QLendApi.Models.Employer", "EmployerNumberNavigation")
                        .WithMany("ForeignWorkers")
                        .HasForeignKey("EmployerNumber")
                        .HasConstraintName("FK__ForeignWo__Emplo__4D94879B");

                    b.HasOne("QLendApi.Models.IncomeInformation", "IncomeNumberNavigation")
                        .WithMany("ForeignWorkers")
                        .HasForeignKey("IncomeNumber")
                        .HasConstraintName("FK__ForeignWo__Incom__4CA06362");

                    b.HasOne("QLendApi.Models.Certificate", "UinoNavigation")
                        .WithMany("ForeignWorkers")
                        .HasForeignKey("Uino")
                        .HasConstraintName("FK__ForeignWo__Emplo__4BAC3F29")
                        .IsRequired();

                    b.Navigation("EmployerNumberNavigation");

                    b.Navigation("IncomeNumberNavigation");

                    b.Navigation("UinoNavigation");
                });

            modelBuilder.Entity("QLendApi.Models.LoanRecord", b =>
                {
                    b.HasOne("QLendApi.Models.ForeignWorker", "IdNavigation")
                        .WithMany("LoanRecords")
                        .HasForeignKey("Id")
                        .HasConstraintName("FK__LoanRecord__ID__5070F446");

                    b.HasOne("QLendApi.Models.CreditSide", "IdnoNavigation")
                        .WithMany("LoanRecords")
                        .HasForeignKey("Idno")
                        .HasConstraintName("FK__LoanRecord__IDNo__5165187F");

                    b.Navigation("IdNavigation");

                    b.Navigation("IdnoNavigation");
                });

            modelBuilder.Entity("QLendApi.Models.RepaymentRecord", b =>
                {
                    b.HasOne("QLendApi.Models.LoanRecord", "LoanNumberNavigation")
                        .WithMany("RepaymentRecords")
                        .HasForeignKey("LoanNumber")
                        .HasConstraintName("FK__Repayment__LoanN__5441852A")
                        .IsRequired();

                    b.Navigation("LoanNumberNavigation");
                });

            modelBuilder.Entity("QLendApi.Models.Certificate", b =>
                {
                    b.Navigation("ForeignWorkers");
                });

            modelBuilder.Entity("QLendApi.Models.CompanyList", b =>
                {
                    b.Navigation("Employers");
                });

            modelBuilder.Entity("QLendApi.Models.CreditSide", b =>
                {
                    b.Navigation("LoanRecords");
                });

            modelBuilder.Entity("QLendApi.Models.Employer", b =>
                {
                    b.Navigation("ForeignWorkers");
                });

            modelBuilder.Entity("QLendApi.Models.ForeignWorker", b =>
                {
                    b.Navigation("LoanRecords");
                });

            modelBuilder.Entity("QLendApi.Models.IncomeInformation", b =>
                {
                    b.Navigation("ForeignWorkers");
                });

            modelBuilder.Entity("QLendApi.Models.LoanRecord", b =>
                {
                    b.Navigation("RepaymentRecords");
                });
#pragma warning restore 612, 618
        }
    }
}

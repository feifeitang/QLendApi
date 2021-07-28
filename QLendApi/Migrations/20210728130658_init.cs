using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    UINo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "統一證號"),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true, comment: "核發日期"),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: true, comment: "居留期限"),
                    ResidencePurpose = table.Column<string>(type: "nchar(70)", fixedLength: true, maxLength: 70, nullable: true, comment: "居留事由，由中台填"),
                    BarcodeNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true, comment: "居留證流水號"),
                    FrontARC = table.Column<byte[]>(type: "image", nullable: true, comment: "居留證正面照片"),
                    BackARC = table.Column<byte[]>(type: "image", nullable: true, comment: "居留證反面照片"),
                    SelfileARC = table.Column<byte[]>(type: "image", nullable: true, comment: "自己與居留證合照照片"),
                    FrontARC2 = table.Column<byte[]>(type: "image", nullable: true),
                    BackARC2 = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Certific__B1FE46809DDC58EA", x => x.UINo);
                },
                comment: "居留證");

            migrationBuilder.CreateTable(
                name: "CompanyList",
                columns: table => new
                {
                    CompanyCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false, comment: "上市櫃代碼"),
                    CompanyName = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: false, comment: "公司名稱")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CompanyL__11A0134A2607838A", x => x.CompanyCode);
                },
                comment: "公司名單");

            migrationBuilder.CreateTable(
                name: "CreditSide",
                columns: table => new
                {
                    IDNo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "身分證"),
                    CreditName = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false, comment: "姓名"),
                    PhoneNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "手機號碼"),
                    Email = table.Column<string>(type: "char(50)", unicode: false, fixedLength: true, maxLength: 50, nullable: false, comment: "電子郵件"),
                    UserName = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false, comment: "使用者名稱"),
                    CreditPassword = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false, comment: "密碼"),
                    BankNumber = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false, comment: "銀行代碼"),
                    AccountNumber = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: false, comment: "銀行帳號"),
                    CreditSignature = table.Column<byte[]>(type: "image", nullable: false, comment: "手寫簽名"),
                    CreditSignature2 = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CreditSi__B87DC9AB36CAC589", x => x.IDNo);
                },
                comment: "貸方");

            migrationBuilder.CreateTable(
                name: "IncomeInformation",
                columns: table => new
                {
                    IncomeNumber = table.Column<int>(type: "int", nullable: false, comment: "收入資訊流水編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvgMonthlyIncome = table.Column<int>(type: "int", nullable: false, comment: "平均月收入 1:17000~24000 2:24000~3000 3:30000以上"),
                    LatePay = table.Column<int>(type: "int", nullable: false, comment: "薪資如期撥入 1:3次以上 2:1~2次 3:從未延發"),
                    PayWay = table.Column<int>(type: "int", nullable: false, comment: "薪資撥付方式 0:現金領取 1:銀行入賬"),
                    RemittanceWay = table.Column<int>(type: "int", nullable: false, comment: "薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款"),
                    FrontSalaryPassbook = table.Column<byte[]>(type: "image", nullable: true, comment: "薪資存摺本正面照片"),
                    InsideSalarybook = table.Column<byte[]>(type: "image", nullable: true, comment: "薪資存摺本內部照片")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__IncomeIn__A42D1F407AC301EC", x => x.IncomeNumber);
                },
                comment: "收入資訊");

            migrationBuilder.CreateTable(
                name: "Employer",
                columns: table => new
                {
                    EmployerNo = table.Column<int>(type: "int", nullable: false, comment: "雇主編號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true, comment: "雇主名字/公司名"),
                    ListedOrNot = table.Column<bool>(type: "bit", nullable: true, comment: "是否上市櫃"),
                    CapitalMoreThan30m = table.Column<bool>(type: "bit", nullable: true, comment: "是否資本額3000萬以上"),
                    CompanyCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true, comment: "上市櫃代碼")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employer__CA446ACE1D7D0603", x => x.EmployerNo);
                    table.ForeignKey(
                        name: "FK__Employer__Compan__3D5E1FD2",
                        column: x => x.CompanyCode,
                        principalTable: "CompanyList",
                        principalColumn: "CompanyCode",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "雇主/公司");

            migrationBuilder.CreateTable(
                name: "ForeignWorker",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "外勞會員流水號")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true, comment: "中文名字"),
                    EnglishName = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true, comment: "英文名字"),
                    Nationality = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: true, comment: "國籍"),
                    Sex = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true, comment: "性別 M:男生 F:女生"),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true, comment: "出生日期"),
                    PhoneNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "手機號碼"),
                    Password = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: false, comment: "密碼 最少8碼 英文和數字(英文不用限制大小寫)"),
                    Marriage = table.Column<int>(type: "int", nullable: true, comment: "婚姻狀態 1:單身 2:已婚"),
                    ImmediateFamilyNumber = table.Column<int>(type: "int", nullable: true, comment: "直系親屬數"),
                    EducationBackground = table.Column<int>(type: "int", nullable: true, comment: "最高學歷 1:國小畢業 2:國中畢業 3:高中職(含)以上"),
                    TimeInTaiwan = table.Column<int>(type: "int", nullable: true, comment: "來台時長 1:未滿1年 2:1~未滿3年 3:9~12年 4:3~9年"),
                    UserName = table.Column<string>(type: "char(20)", unicode: false, fixedLength: true, maxLength: 20, nullable: true, comment: "使用者名稱"),
                    PassportNumber = table.Column<string>(type: "char(9)", unicode: false, fixedLength: true, maxLength: 9, nullable: true, comment: "護照號碼"),
                    Signature = table.Column<byte[]>(type: "image", nullable: true, comment: "手寫簽名"),
                    CreditScore = table.Column<int>(type: "int", nullable: true, comment: "信評分數 1~5"),
                    DefaultRisk = table.Column<int>(type: "int", nullable: true, comment: "違約風險 1~10"),
                    BankNumber = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true, comment: "銀行代碼"),
                    AccountNumber = table.Column<string>(type: "char(14)", unicode: false, fixedLength: true, maxLength: 14, nullable: true, comment: "銀行帳號"),
                    RegisterTime = table.Column<DateTime>(type: "date", nullable: true, comment: "會員註冊審核狀態 0:審核中 1:審核通過"),
                    State = table.Column<int>(type: "int", nullable: true, comment: "會員註冊時間 有更新(資料建立、審查通過/沒過)都改這個時間"),
                    KindOfWork = table.Column<int>(type: "int", nullable: true, comment: "工作性質 1:社福移工 2:產業移工 3:其他"),
                    Workplace = table.Column<int>(type: "int", nullable: true, comment: "工作地點 1:基隆市 2:台北市 3:新北市 4:桃園市 5:新竹市 6:新竹縣 7:苗栗縣 8:南投縣 9:台中市 10:彰化縣 11:雲林縣 12:嘉義縣 13:嘉義市 14:台南市 15:高雄市 16:屏東縣 17:宜蘭縣 18:花蓮縣 19:台東縣 20:澎湖縣"),
                    UINo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "統一證號"),
                    IncomeNumber = table.Column<int>(type: "int", nullable: true, comment: "收入流水編號"),
                    EmployerNumber = table.Column<int>(type: "int", nullable: true, comment: "雇主編號"),
                    Signature2 = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignWorker", x => x.ID);
                    table.ForeignKey(
                        name: "FK__ForeignWo__Emplo__4BAC3F29",
                        column: x => x.UINo,
                        principalTable: "Certificate",
                        principalColumn: "UINo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ForeignWo__Emplo__4D94879B",
                        column: x => x.EmployerNumber,
                        principalTable: "Employer",
                        principalColumn: "EmployerNo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__ForeignWo__Incom__4CA06362",
                        column: x => x.IncomeNumber,
                        principalTable: "IncomeInformation",
                        principalColumn: "IncomeNumber",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "外勞");

            migrationBuilder.CreateTable(
                name: "LoanRecord",
                columns: table => new
                {
                    LoanNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "借貸編號"),
                    LoanDate = table.Column<DateTime>(type: "date", nullable: true, comment: "借款日期"),
                    Amount = table.Column<int>(type: "int", nullable: false, comment: "金額"),
                    Period = table.Column<int>(type: "int", nullable: false, comment: "期數"),
                    InterestRate = table.Column<double>(type: "float", nullable: true, comment: "利率"),
                    Purpose = table.Column<int>(type: "int", nullable: false, comment: "用途 1:其他 2:投資理財 3:個人消費(食衣住行) 4:家鄉急用"),
                    State = table.Column<int>(type: "int", nullable: true, comment: "借款狀態 0:審核中 1:允許借款 2:確認借款 3:已匯款 -1:拒絕借款"),
                    Auditor = table.Column<int>(type: "int", nullable: true, comment: "審核者 0:系統 1:人工"),
                    ID = table.Column<int>(type: "int", nullable: true, comment: "外勞會員流水號"),
                    IDNo = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true, comment: "身分證(貸方)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoanReco__EEC2662937B8C5C8", x => x.LoanNumber);
                    table.ForeignKey(
                        name: "FK__LoanRecord__ID__5070F446",
                        column: x => x.ID,
                        principalTable: "ForeignWorker",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__LoanRecord__IDNo__5165187F",
                        column: x => x.IDNo,
                        principalTable: "CreditSide",
                        principalColumn: "IDNo",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "借貸紀錄");

            migrationBuilder.CreateTable(
                name: "RepaymentRecord",
                columns: table => new
                {
                    RepaymentNumber = table.Column<string>(type: "char(11)", unicode: false, fixedLength: true, maxLength: 11, nullable: false, comment: "還款編號"),
                    RepaymentOrder = table.Column<int>(type: "int", nullable: false, comment: "還款序號"),
                    RepaymentDate = table.Column<DateTime>(type: "date", nullable: false, comment: "還款日期"),
                    ActualRepaymentDate = table.Column<DateTime>(type: "date", nullable: true, comment: "實際還款日"),
                    RepaymentAmount = table.Column<int>(type: "int", nullable: false, comment: "還款金額"),
                    ActualRepaymentAmount = table.Column<int>(type: "int", nullable: true, comment: "實際還款金額"),
                    RepaymentBarCode = table.Column<byte[]>(type: "image", nullable: true, comment: "還款條碼"),
                    State = table.Column<int>(type: "int", nullable: false, comment: "還款狀態 -1:expired 0:unpaid 1:paid"),
                    LoanNumber = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false, comment: "借貸編號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Repaymen__80FF429F3AC78ED1", x => x.RepaymentNumber);
                    table.ForeignKey(
                        name: "FK__Repayment__LoanN__5441852A",
                        column: x => x.LoanNumber,
                        principalTable: "LoanRecord",
                        principalColumn: "LoanNumber",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "還款紀錄");

            migrationBuilder.CreateIndex(
                name: "IX_Employer_CompanyCode",
                table: "Employer",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignWorker_EmployerNumber",
                table: "ForeignWorker",
                column: "EmployerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignWorker_IncomeNumber",
                table: "ForeignWorker",
                column: "IncomeNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignWorker_UINo",
                table: "ForeignWorker",
                column: "UINo");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRecord_ID",
                table: "LoanRecord",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_LoanRecord_IDNo",
                table: "LoanRecord",
                column: "IDNo");

            migrationBuilder.CreateIndex(
                name: "IX_RepaymentRecord_LoanNumber",
                table: "RepaymentRecord",
                column: "LoanNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepaymentRecord");

            migrationBuilder.DropTable(
                name: "LoanRecord");

            migrationBuilder.DropTable(
                name: "ForeignWorker");

            migrationBuilder.DropTable(
                name: "CreditSide");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "Employer");

            migrationBuilder.DropTable(
                name: "IncomeInformation");

            migrationBuilder.DropTable(
                name: "CompanyList");
        }
    }
}

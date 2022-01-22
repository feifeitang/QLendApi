using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class modifyIncomeInfoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RemittanceWay",
                table: "IncomeInformation",
                type: "int",
                nullable: true,
                comment: "薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款");

            migrationBuilder.AlterColumn<int>(
                name: "PayWay",
                table: "IncomeInformation",
                type: "int",
                nullable: true,
                comment: "薪資撥付方式 0:現金領取 1:銀行入賬",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "薪資撥付方式 0:現金領取 1:銀行入賬");

            migrationBuilder.AlterColumn<int>(
                name: "LatePay",
                table: "IncomeInformation",
                type: "int",
                nullable: true,
                comment: "薪資如期撥入 1:3次以上 2:1~2次 3:從未延發",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "薪資如期撥入 1:3次以上 2:1~2次 3:從未延發");

            migrationBuilder.AlterColumn<int>(
                name: "AvgMonthlyIncome",
                table: "IncomeInformation",
                type: "int",
                nullable: true,
                comment: "平均月收入 1:17000~24000 2:24000~3000 3:30000以上",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "平均月收入 1:17000~24000 2:24000~3000 3:30000以上");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RemittanceWay",
                table: "IncomeInformation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "薪資結匯方式 0:外勞商店匯款 1:其他APP匯款 2:QPAY匯款");

            migrationBuilder.AlterColumn<int>(
                name: "PayWay",
                table: "IncomeInformation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "薪資撥付方式 0:現金領取 1:銀行入賬",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "薪資撥付方式 0:現金領取 1:銀行入賬");

            migrationBuilder.AlterColumn<int>(
                name: "LatePay",
                table: "IncomeInformation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "薪資如期撥入 1:3次以上 2:1~2次 3:從未延發",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "薪資如期撥入 1:3次以上 2:1~2次 3:從未延發");

            migrationBuilder.AlterColumn<int>(
                name: "AvgMonthlyIncome",
                table: "IncomeInformation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "平均月收入 1:17000~24000 2:24000~3000 3:30000以上",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "平均月收入 1:17000~24000 2:24000~3000 3:30000以上");
        }
    }
}

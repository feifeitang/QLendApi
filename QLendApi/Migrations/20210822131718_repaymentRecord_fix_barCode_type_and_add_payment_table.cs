using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class repaymentRecord_fix_barCode_type_and_add_payment_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BarCode3",
                table: "RepaymentRecord",
                type: "char(20)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BarCode2",
                table: "RepaymentRecord",
                type: "char(20)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BarCode1",
                table: "RepaymentRecord",
                type: "char(20)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
            name: "Payment",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false, comment: "Payment id")
                    .Annotation("SqlServer:Identity", "1, 1"),
                MerchantTradeNo = table.Column<string>(type: "char(20)", fixedLength: false, maxLength: 20, nullable: false, comment: "MerchantTradeNo"),
                Amount = table.Column<int>(type: "int", nullable: false, comment: "Amount"),
                ReturnURL = table.Column<string>(type: "nvarchar(200)", fixedLength: true, maxLength: 200, nullable: false, comment: "ecpay receive pay return result"),
                PaymentInfoURL = table.Column<string>(type: "nvarchar(200)", fixedLength: true, maxLength: 200, nullable: false, comment: "order create ecpay send barcode info url"),
                BarCode1 = table.Column<string>(type: "char(20)", fixedLength: false, maxLength: 20, nullable: true),
                BarCode2 = table.Column<string>(type: "char(20)", fixedLength: false, maxLength: 20, nullable: true),
                BarCode3 = table.Column<string>(type: "char(20)", fixedLength: false, maxLength: 20, nullable: true),
                TradeNo = table.Column<string>(type: "char(20)", fixedLength: false, maxLength: 20, nullable: true, comment: "ecpay TradeNo"),
                Status = table.Column<int>(type: "int", nullable: false, comment: "payemnt status"),
                CreateTime = table.Column<DateTime>(type: "date", nullable: false, comment: "first create record time"),
                UpdateTime = table.Column<DateTime>(type: "date", nullable: true, comment: "record update will override"),
                ReceivePayTime = table.Column<DateTime>(type: "date", nullable: true, comment: "receive ecpay pay result insert"),
                RepaymentNumber = table.Column<string>(type: "char(11)", fixedLength: false, maxLength: 11, nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payment", x => x.ID);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "BarCode3",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "BarCode2",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "BarCode1",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(20)",
                oldNullable: true);

            migrationBuilder.DropColumn(
                    name: "Status",
                    table: "Payment");
        }
    }
}

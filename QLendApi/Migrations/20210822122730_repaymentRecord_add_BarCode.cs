using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class repaymentRecord_add_BarCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepaymentBarCode",
                table: "RepaymentRecord");

            migrationBuilder.AddColumn<byte[]>(
                name: "BarCode1",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "BarCode2",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "BarCode3",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode1",
                table: "RepaymentRecord");

            migrationBuilder.DropColumn(
                name: "BarCode2",
                table: "RepaymentRecord");

            migrationBuilder.DropColumn(
                name: "BarCode3",
                table: "RepaymentRecord");

            migrationBuilder.AddColumn<byte[]>(
                name: "RepaymentBarCode",
                table: "RepaymentRecord",
                type: "image",
                nullable: true,
                comment: "還款條碼");
        }
    }
}

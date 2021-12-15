using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class repaymentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckTime",
                table: "RepaymentRecord",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Delay",
                table: "RepaymentRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Receipt",
                table: "RepaymentRecord",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemitAccount",
                table: "RepaymentRecord",
                type: "char(5)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckTime",
                table: "RepaymentRecord");

            migrationBuilder.DropColumn(
                name: "Delay",
                table: "RepaymentRecord");

            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "RepaymentRecord");

            migrationBuilder.DropColumn(
                name: "RemitAccount",
                table: "RepaymentRecord");
        }
    }
}

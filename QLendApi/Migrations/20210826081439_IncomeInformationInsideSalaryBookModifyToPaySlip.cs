using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class IncomeInformationInsideSalaryBookModifyToPaySlip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsideSalarybook",
                table: "IncomeInformation");

            migrationBuilder.AddColumn<byte[]>(
                name: "PaySlip",
                table: "IncomeInformation",
                type: "image",
                nullable: true,
                comment: "薪資條");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaySlip",
                table: "IncomeInformation");

            migrationBuilder.AddColumn<byte[]>(
                name: "InsideSalarybook",
                table: "IncomeInformation",
                type: "image",
                nullable: true,
                comment: "薪資存摺本內部照片");
        }
    }
}

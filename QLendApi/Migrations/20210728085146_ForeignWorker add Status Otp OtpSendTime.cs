using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class ForeignWorkeraddStatusOtpOtpSendTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OTP",
                table: "ForeignWorker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPSendTIme",
                table: "ForeignWorker",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ForeignWorker",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTP",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "OTPSendTIme",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ForeignWorker");
        }
    }
}

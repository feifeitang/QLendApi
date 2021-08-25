using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class FpreignWorkerEditPersonalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "ForeignWorker");

            migrationBuilder.AddColumn<string>(
                name: "CommunicationSoftwareAccount",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookAccount",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyMemberName",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyMemberPhoneNumber",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropColumn(
                name: "CommunicationSoftwareAccount",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "FacebookAccount",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "FamilyMemberName",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "FamilyMemberPhoneNumber",
                table: "ForeignWorker");

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "ForeignWorker",
                type: "char(9)",
                unicode: false,
                fixedLength: true,
                maxLength: 9,
                nullable: true,
                comment: "護照號碼");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class ForeignWorkerAddPassportAndLocalIdCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "LocalIdCard",
                table: "ForeignWorker",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Passport",
                table: "ForeignWorker",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNumber",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalIdCard",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "Passport",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "PassportNumber",
                table: "ForeignWorker");
        }
    }
}

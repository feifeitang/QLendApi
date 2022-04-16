using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class score2Risk2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditScore2",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRisk2",
                table: "ForeignWorker",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditScore2",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "DefaultRisk2",
                table: "ForeignWorker");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class modelNewCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultRisk2",
                table: "ForeignWorker",
                newName: "DefaultRiskSvmOnehot");

            migrationBuilder.RenameColumn(
                name: "CreditScore2",
                table: "ForeignWorker",
                newName: "DefaultRiskSvm");

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreOnehot",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreRf",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreRfOnehot",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreSvm",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditScoreSvmOnehot",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRiskOnehot",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRiskRf",
                table: "ForeignWorker",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultRiskRfOnehot",
                table: "ForeignWorker",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditScoreOnehot",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "CreditScoreRf",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "CreditScoreRfOnehot",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "CreditScoreSvm",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "CreditScoreSvmOnehot",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "DefaultRiskOnehot",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "DefaultRiskRf",
                table: "ForeignWorker");

            migrationBuilder.DropColumn(
                name: "DefaultRiskRfOnehot",
                table: "ForeignWorker");

            migrationBuilder.RenameColumn(
                name: "DefaultRiskSvmOnehot",
                table: "ForeignWorker",
                newName: "DefaultRisk2");

            migrationBuilder.RenameColumn(
                name: "DefaultRiskSvm",
                table: "ForeignWorker",
                newName: "CreditScore2");
        }
    }
}

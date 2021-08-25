using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class ForeignWorkerAddCommunicationSoftware : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommunicationSoftware",
                table: "ForeignWorker",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunicationSoftware",
                table: "ForeignWorker");
        }
    }
}

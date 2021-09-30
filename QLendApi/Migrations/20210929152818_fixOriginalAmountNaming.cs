using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class fixOriginalAmountNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrginalAmount",
                table: "RepaymentRecord",
                newName: "OriginalAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalAmount",
                table: "RepaymentRecord",
                newName: "OrginalAmount");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QLendApi.Migrations
{
    public partial class noticeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "Notice id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nchar(200)", fixedLength: true, maxLength: 200, nullable: true, comment: "Notice Content"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Link = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "date", nullable: false),
                    ForeignWorkerId = table.Column<int>(type: "int", nullable: false, comment: "外勞會員流水號")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Notice__ForeignWo__4BAC3F29",
                        column: x => x.ForeignWorkerId,
                        principalTable: "ForeignWorker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                            name: "Status",
                            table: "Notice");
        }
    }
}

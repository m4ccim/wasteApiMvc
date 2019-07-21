using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class rero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BillProductWastes_WasteId",
                table: "BillProductWastes",
                column: "WasteId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillProductWastes_Wastes_WasteId",
                table: "BillProductWastes",
                column: "WasteId",
                principalTable: "Wastes",
                principalColumn: "WasteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillProductWastes_Wastes_WasteId",
                table: "BillProductWastes");

            migrationBuilder.DropIndex(
                name: "IX_BillProductWastes_WasteId",
                table: "BillProductWastes");
        }
    }
}

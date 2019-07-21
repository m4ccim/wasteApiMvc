using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class balances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wasteBalances_Cards_CardId",
                table: "wasteBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_wasteBalances_Wastes_WasteId",
                table: "wasteBalances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wasteBalances",
                table: "wasteBalances");

            migrationBuilder.RenameTable(
                name: "wasteBalances",
                newName: "WasteBalances");

            migrationBuilder.RenameIndex(
                name: "IX_wasteBalances_WasteId",
                table: "WasteBalances",
                newName: "IX_WasteBalances_WasteId");

            migrationBuilder.RenameIndex(
                name: "IX_wasteBalances_CardId",
                table: "WasteBalances",
                newName: "IX_WasteBalances_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteBalances",
                table: "WasteBalances",
                column: "BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_WasteBalances_Cards_CardId",
                table: "WasteBalances",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WasteBalances_Wastes_WasteId",
                table: "WasteBalances",
                column: "WasteId",
                principalTable: "Wastes",
                principalColumn: "WasteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WasteBalances_Cards_CardId",
                table: "WasteBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_WasteBalances_Wastes_WasteId",
                table: "WasteBalances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteBalances",
                table: "WasteBalances");

            migrationBuilder.RenameTable(
                name: "WasteBalances",
                newName: "wasteBalances");

            migrationBuilder.RenameIndex(
                name: "IX_WasteBalances_WasteId",
                table: "wasteBalances",
                newName: "IX_wasteBalances_WasteId");

            migrationBuilder.RenameIndex(
                name: "IX_WasteBalances_CardId",
                table: "wasteBalances",
                newName: "IX_wasteBalances_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wasteBalances",
                table: "wasteBalances",
                column: "BalanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_wasteBalances_Cards_CardId",
                table: "wasteBalances",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wasteBalances_Wastes_WasteId",
                table: "wasteBalances",
                column: "WasteId",
                principalTable: "Wastes",
                principalColumn: "WasteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

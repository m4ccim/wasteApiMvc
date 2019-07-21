using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class Billpro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_BillProducts_BillProductId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_BillProductId",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillProductId",
                table: "Bills");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Bills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "BillProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BillProducts_BillId",
                table: "BillProducts",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillProducts_Bills_BillId",
                table: "BillProducts",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "BillId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillProducts_Bills_BillId",
                table: "BillProducts");

            migrationBuilder.DropIndex(
                name: "IX_BillProducts_BillId",
                table: "BillProducts");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "BillProducts");

            migrationBuilder.AddColumn<int>(
                name: "BillProductId",
                table: "Bills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillProductId",
                table: "Bills",
                column: "BillProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_BillProducts_BillProductId",
                table: "Bills",
                column: "BillProductId",
                principalTable: "BillProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

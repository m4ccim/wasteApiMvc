using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class fulldb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RecyclingPrice",
                table: "Wastes",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "WasteBalances",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.CreateTable(
                name: "BillProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillProductWastes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillProductId = table.Column<int>(nullable: false),
                    WasteId = table.Column<int>(nullable: false),
                    DiscountedAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillProductWastes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillProductWastes_BillProducts_BillProductId",
                        column: x => x.BillProductId,
                        principalTable: "BillProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    BillProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_BillProducts_BillProductId",
                        column: x => x.BillProductId,
                        principalTable: "BillProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillProducts_ProductId",
                table: "BillProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BillProductWastes_BillProductId",
                table: "BillProductWastes",
                column: "BillProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillProductId",
                table: "Bills",
                column: "BillProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CardId",
                table: "Bills",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillProductWastes");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "BillProducts");

            migrationBuilder.AlterColumn<float>(
                name: "RecyclingPrice",
                table: "Wastes",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "WasteBalances",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}

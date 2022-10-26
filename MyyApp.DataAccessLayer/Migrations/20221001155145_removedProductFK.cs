using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyyApp.DataAccessLayer.Migrations
{
    public partial class removedProductFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_ProductDbs_ProductDbId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_ProductDbId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ProductDbId",
                table: "ShoppingCarts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDbId",
                table: "ShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductDbId",
                table: "ShoppingCarts",
                column: "ProductDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_ProductDbs_ProductDbId",
                table: "ShoppingCarts",
                column: "ProductDbId",
                principalTable: "ProductDbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

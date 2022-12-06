using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyyApp.DataAccessLayer.Migrations
{
    public partial class AddOrderDetailToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_ProductDbs_ProductDbId",
                table: "orderDetails");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_ProductDbId",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "ProductDbId",
                table: "orderDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductDbId",
                table: "orderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_ProductDbId",
                table: "orderDetails",
                column: "ProductDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_ProductDbs_ProductDbId",
                table: "orderDetails",
                column: "ProductDbId",
                principalTable: "ProductDbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

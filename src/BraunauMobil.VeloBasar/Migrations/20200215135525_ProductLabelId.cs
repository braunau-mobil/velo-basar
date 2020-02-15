using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class ProductLabelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DropColumn(
                name: "Label",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "LabelId",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Label",
                table: "Products",
                type: "integer",
                nullable: true);
        }
    }
}

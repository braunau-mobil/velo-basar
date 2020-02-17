using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class ProductTypeDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductTypes");
        }
    }
}

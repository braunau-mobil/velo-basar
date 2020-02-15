using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class RemoveBicAndAddPhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BIC",
                table: "Sellers");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Sellers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Sellers");

            migrationBuilder.AddColumn<string>(
                name: "BIC",
                table: "Sellers",
                type: "text",
                nullable: true);
        }
    }
}

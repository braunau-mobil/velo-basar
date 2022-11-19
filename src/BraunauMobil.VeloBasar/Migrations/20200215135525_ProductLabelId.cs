using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class ProductLabelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.RenameColumn(
                name: "Label",
                table: "Products",
                newName: "LabelId");

            migrationBuilder.AlterColumn<int>(
                name: "LabelId",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.AlterColumn<int>(
                name: "LabelId",
                table: "Products",
                nullable: true,
                defaultValue: null);
            
            migrationBuilder.RenameColumn(
                name: "LabelId",
                table: "Products",
                newName: "Label");
        }
    }
}

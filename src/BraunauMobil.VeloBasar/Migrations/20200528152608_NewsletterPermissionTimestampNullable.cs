using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class NewsletterPermissionTimestampNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NewsletterPermissionTimesStamp",
                table: "Sellers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NewsletterPermissionTimesStamp",
                table: "Sellers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}

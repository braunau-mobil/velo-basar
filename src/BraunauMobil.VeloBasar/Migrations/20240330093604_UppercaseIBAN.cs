using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraunauMobil.VeloBasar.Migrations
{
    /// <inheritdoc />
    public partial class UppercaseIBAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.Sql("""UPDATE "Sellers" SET "IBAN" = UPPER("IBAN")""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

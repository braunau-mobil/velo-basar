using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BraunauMobil.VeloBasar.Migrations
{
    /// <inheritdoc />
    public partial class UnsettlementNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            migrationBuilder.Sql("""INSERT INTO "Numbers" ("BasarId", "Type", "Value") SELECT "BasarId", 7, 0 FROM "Numbers" WHERE "Type" = 0""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { }
    }
}

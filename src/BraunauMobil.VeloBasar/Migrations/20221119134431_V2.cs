using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable
namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class V2
        : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);

            BasarsUp(migrationBuilder);
            BrandsUp(migrationBuilder);
            CountriesUp(migrationBuilder);
            ProductTypesUp(migrationBuilder);
            ZipCodesUp(migrationBuilder);

            migrationBuilder.Sql(@"DELETE FROM ""Files""");
            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "Files",
                type: "bytea",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "bytea",
                oldNullable: true);
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DonateIfNotSold",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""ValueState"" = 1 WHERE ""ValueState"" = 10");
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""StorageState"" = 1 WHERE ""StorageState"" = 0");
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""StorageState"" = 2 WHERE ""StorageState"" = 10");
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""StorageState"" = 3 WHERE ""StorageState"" = 20");
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""StorageState"" = 4 WHERE ""StorageState"" = 30");

            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 6 WHERE ""Type"" = 6");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 5 WHERE ""Type"" = 5");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 4 WHERE ""Type"" = 4");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 2 WHERE ""Type"" = 3");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 1 WHERE ""Type"" = 2");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 3 WHERE ""Type"" = 1");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""Type"" = 0 WHERE ""Type"" = 0");
            migrationBuilder.Sql(@"UPDATE ""Transactions"" SET ""DocumentId"" = NULL");
            migrationBuilder.AddColumn<int>(
                name: "ParentTransactionId",
                table: "Transactions",
                type: "integer",
                nullable: true);
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ParentTransactionId",
                table: "Transactions",
                column: "ParentTransactionId");
            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_ParentTransactionId",
                table: "Transactions",
                column: "ParentTransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "Sellers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Sellers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.Sql(@"UPDATE ""Sellers"" SET ""State"" = 1");
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sellers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sellers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.Sql(@"UPDATE ""Sellers"" SET ""ValueState"" = 1 WHERE ""ValueState"" = 10");

            migrationBuilder.CreateTable(
                name: "AcceptSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BasarId = table.Column<int>(type: "integer", nullable: false),
                    StartTimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndTimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
        constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcceptSessions_Basars_BasarId",
                        column: x => x.BasarId,
                        principalTable: "Basars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcceptSessions_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<DateTime>(
                name: "TEMP_MigratedFromTransaction",
                table: "AcceptSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
INSERT INTO ""AcceptSessions""
(""BasarId"", ""StartTimeStamp"", ""EndTimeStamp"", ""SellerId"", ""TEMP_MigratedFromTransaction"")
SELECT ""Transactions"".""BasarId"", ""Transactions"".""TimeStamp"", ""Transactions"".""TimeStamp"", ""Transactions"".""SellerId"", ""Transactions"".""Id""
FROM ""Transactions"" WHERE ""Transactions"".""Type"" = 0
");

            migrationBuilder.Sql(@"UPDATE ""AcceptSessions"" SET ""State"" = 1");

            migrationBuilder.Sql(@"
UPDATE ""Products""
SET ""SessionId"" = ""AcceptSessions"".""Id""
FROM ""ProductToTransaction"" 
INNER JOIN ""Transactions"" ON ""Transactions"".""Id"" = ""ProductToTransaction"".""TransactionId"" AND ""Transactions"".""Type"" = 0
INNER JOIN ""AcceptSessions"" ON ""AcceptSessions"".""TEMP_MigratedFromTransaction"" = ""Transactions"".""Id""
WHERE ""ProductToTransaction"".""ProductId"" = ""Products"".""Id""
");

            migrationBuilder.DropColumn(
                name: "TEMP_MigratedFromTransaction",
                table: "AcceptSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Basars_BasarId",
                table: "Products");
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Sellers_SellerId",
                table: "Products");
            migrationBuilder.DropIndex(
                name: "IX_Products_BasarId",
                table: "Products");
            migrationBuilder.DropIndex(
                name: "IX_Products_SellerId",
                table: "Products");
            migrationBuilder.DropColumn(
                name: "BasarId",
                table: "Products");
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Products");
            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AcceptSessions_SessionId",
                table: "Products",
                column: "SessionId",
                principalTable: "AcceptSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.CreateIndex(
                name: "IX_AcceptSessions_BasarId",
                table: "AcceptSessions",
                column: "BasarId");
            migrationBuilder.CreateIndex(
                name: "IX_AcceptSessions_SellerId",
                table: "AcceptSessions",
                column: "SellerId");
            migrationBuilder.CreateIndex(
                name: "IX_Products_SessionId",
                table: "Products",
                column: "SessionId");
        }

        private static void BasarsUp(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Basars",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Basars",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Basars",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Basars",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(@"UPDATE ""Basars"" SET ""CreatedAt"" = ""Date"", ""UpdatedAt"" = ""Date""");
        }

        private static void BrandsUp(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Brands",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Brands",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(@"UPDATE ""Brands"" SET ""State"" = 1 WHERE ""State"" = 0");
            migrationBuilder.Sql(@"UPDATE ""Brands"" SET ""State"" = 0 WHERE ""State"" = 10");
        }

        private static void CountriesUp(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Iso3166Alpha3Code",
                table: "Countries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Countries",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.Sql(@"UPDATE ""Countries"" SET ""State"" = 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Countries",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Countries",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }        

        private static void ProductTypesUp(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductTypes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductTypes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql(@"UPDATE ""ProductTypes"" SET ""State"" = 1 WHERE ""State"" = 0");
            migrationBuilder.Sql(@"UPDATE ""ProductTypes"" SET ""State"" = 0 WHERE ""State"" = 10");
        }

        private static void ZipCodesUp(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ZipMap",
                newName: "ZipCodes");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "ZipCodes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
            => throw new NotSupportedException();
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BraunauMobil.VeloBasar.Migrations
{
    public partial class SellerExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("Sellers", "PhoneNumber", null, "PhoneNumber", "");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Sellers",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Sellers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EMail",
                table: "Sellers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasNewsletterPermission",
                table: "Sellers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsletterPermissionTimesStamp",
                table: "Sellers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "EMail",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "HasNewsletterPermission",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "NewsletterPermissionTimesStamp",
                table: "Sellers");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Sellers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}

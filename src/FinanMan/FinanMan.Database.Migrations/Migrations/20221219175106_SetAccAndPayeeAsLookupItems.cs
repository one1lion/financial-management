using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanMan.Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SetAccAndPayeeAsLookupItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Payees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Payees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Payees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Accounts");
        }
    }
}

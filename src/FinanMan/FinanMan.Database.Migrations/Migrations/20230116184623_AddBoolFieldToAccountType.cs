using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanMan.Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddBoolFieldToAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncreaseOnPayment",
                table: "AccountTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IncreaseOnPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "IncreaseOnPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IncreaseOnPayment",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "IncreaseOnPayment",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncreaseOnPayment",
                table: "AccountTypes");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanMan.Database.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SetCascadingRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_DepositReasons_DepositReasonId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_TargetAccountId",
                table: "Transfers");

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IncreaseOnPayment",
                value: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_DepositReasons_DepositReasonId",
                table: "Deposits",
                column: "DepositReasonId",
                principalTable: "DepositReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_TargetAccountId",
                table: "Transfers",
                column: "TargetAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_DepositReasons_DepositReasonId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_TargetAccountId",
                table: "Transfers");

            migrationBuilder.UpdateData(
                table: "AccountTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IncreaseOnPayment",
                value: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_DepositReasons_DepositReasonId",
                table: "Deposits",
                column: "DepositReasonId",
                principalTable: "DepositReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_TargetAccountId",
                table: "Transfers",
                column: "TargetAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}

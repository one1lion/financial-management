using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanMan.Database.Migrations.Migrations;

public partial class AddedFinanTranTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Transactions_Payees_PayeeId",
            table: "Transactions");

        migrationBuilder.DropIndex(
            name: "IX_Transactions_PayeeId",
            table: "Transactions");

        migrationBuilder.DropColumn(
            name: "PayeeId",
            table: "Transactions");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "LineItemTypes",
            type: "nvarchar(80)",
            maxLength: 80,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateTable(
            name: "DepositReasons",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                SortOrder = table.Column<int>(type: "int", nullable: false),
                LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                Deleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DepositReasons", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TransactionId = table.Column<int>(type: "int", nullable: false),
                PayeeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Payees_PayeeId",
                    column: x => x.PayeeId,
                    principalTable: "Payees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Payments_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Deposits",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                TransactionId = table.Column<int>(type: "int", nullable: false),
                DepositReasonId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Deposits", x => x.Id);
                table.ForeignKey(
                    name: "FK_Deposits_DepositReasons_DepositReasonId",
                    column: x => x.DepositReasonId,
                    principalTable: "DepositReasons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Deposits_Transactions_TransactionId",
                    column: x => x.TransactionId,
                    principalTable: "Transactions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_LineItemTypes_Name",
            table: "LineItemTypes",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_DepositReasons_Name",
            table: "DepositReasons",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Deposits_DepositReasonId",
            table: "Deposits",
            column: "DepositReasonId");

        migrationBuilder.CreateIndex(
            name: "IX_Deposits_TransactionId",
            table: "Deposits",
            column: "TransactionId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Payments_PayeeId",
            table: "Payments",
            column: "PayeeId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_TransactionId",
            table: "Payments",
            column: "TransactionId",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Deposits");

        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "DepositReasons");

        migrationBuilder.DropIndex(
            name: "IX_LineItemTypes_Name",
            table: "LineItemTypes");

        migrationBuilder.AddColumn<int>(
            name: "PayeeId",
            table: "Transactions",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "LineItemTypes",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(80)",
            oldMaxLength: 80);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_PayeeId",
            table: "Transactions",
            column: "PayeeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Transactions_Payees_PayeeId",
            table: "Transactions",
            column: "PayeeId",
            principalTable: "Payees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

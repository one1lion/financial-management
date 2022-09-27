using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanMan.Database.Migrations.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountTypes",
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
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
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
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

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
                name: "LineItemTypes",
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
                    table.PrimaryKey("PK_LineItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurrenceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LuCategoryPayee",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    PayeesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuCategoryPayee", x => new { x.CategoriesId, x.PayeesId });
                    table.ForeignKey(
                        name: "FK_LuCategoryPayee_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LuCategoryPayee_Payees_PayeesId",
                        column: x => x.PayeesId,
                        principalTable: "Payees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurrenceTypeId = table.Column<int>(type: "int", nullable: false),
                    DayNum = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    PayeeId = table.Column<int>(type: "int", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledTransactions_Payees_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Payees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduledTransactions_RecurrenceTypes_RecurrenceTypeId",
                        column: x => x.RecurrenceTypeId,
                        principalTable: "RecurrenceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    DateEntered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
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
                    DepositReasonId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
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
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    TargetAccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_TargetAccountId",
                        column: x => x.TargetAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transfers_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    LineItemTypeId = table.Column<int>(type: "int", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_LineItemTypes_LineItemTypeId",
                        column: x => x.LineItemTypeId,
                        principalTable: "LineItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionDetails_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountTypes",
                columns: new[] { "Id", "Deleted", "LastUpdated", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Checking", 1 },
                    { 2, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Savings", 2 },
                    { 3, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Credit Card", 3 },
                    { 4, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Cash", 4 }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Deleted", "LastUpdated", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Grocery Store", 1 },
                    { 2, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "General Goods", 2 },
                    { 3, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Fast Food", 3 },
                    { 4, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Restaraunt", 4 },
                    { 5, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Streaming Serivce", 5 },
                    { 6, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Apothecary", 6 },
                    { 7, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Clothing", 7 }
                });

            migrationBuilder.InsertData(
                table: "LineItemTypes",
                columns: new[] { "Id", "Deleted", "LastUpdated", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Sub Total", 1 },
                    { 2, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Tax", 2 },
                    { 3, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Fee", 3 },
                    { 4, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Surcharge", 4 },
                    { 5, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Shipping", 5 },
                    { 6, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Tip", 6 },
                    { 7, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Refund", 7 },
                    { 8, false, new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Discount", 8 }
                });

            migrationBuilder.InsertData(
                table: "RecurrenceTypes",
                columns: new[] { "Id", "Deleted", "DisplayText", "LastUpdated", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, false, "One-time", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "OneTime", 1 },
                    { 2, false, "Daily", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Daily", 2 },
                    { 3, false, "Weekly", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Weekly", 3 },
                    { 4, false, "Bi-weekly", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "BiWeekly", 4 },
                    { 5, false, "Monthly", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Monthly", 5 },
                    { 6, false, "Bi-monthly", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "BiMonthly", 6 },
                    { 7, false, "Quarterly", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Quarterly", 7 },
                    { 8, false, "Semi-annually", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "SemiAnnually", 8 },
                    { 9, false, "Annually", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Annually", 9 },
                    { 10, false, "Custom...", new DateTime(2022, 7, 25, 17, 41, 0, 0, DateTimeKind.Unspecified), "Custom", 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name",
                table: "Accounts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTypes_Name",
                table: "AccountTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
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
                name: "IX_LineItemTypes_Name",
                table: "LineItemTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LuCategoryPayee_PayeesId",
                table: "LuCategoryPayee",
                column: "PayeesId");

            migrationBuilder.CreateIndex(
                name: "IX_Payees_Name",
                table: "Payees",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PayeeId",
                table: "Payments",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecurrenceTypes_Name",
                table: "RecurrenceTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTransactions_AccountId",
                table: "ScheduledTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTransactions_PayeeId",
                table: "ScheduledTransactions",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTransactions_RecurrenceTypeId",
                table: "ScheduledTransactions",
                column: "RecurrenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_LineItemTypeId",
                table: "TransactionDetails",
                column: "LineItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetails_PaymentId",
                table: "TransactionDetails",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TargetAccountId",
                table: "Transfers",
                column: "TargetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TransactionId",
                table: "Transfers",
                column: "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.DropTable(
                name: "LuCategoryPayee");

            migrationBuilder.DropTable(
                name: "ScheduledTransactions");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "DepositReasons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "RecurrenceTypes");

            migrationBuilder.DropTable(
                name: "LineItemTypes");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Payees");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountTypes");
        }
    }
}

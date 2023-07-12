using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyBillings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyBillings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MoneyCurrency = table.Column<string>(type: "text", nullable: false),
                    Include = table.Column<bool>(type: "boolean", nullable: false),
                    MonthlyBillingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_MonthlyBillings_MonthlyBillingId",
                        column: x => x.MonthlyBillingId,
                        principalTable: "MonthlyBillings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    MoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MoneyCurrency = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<long>(type: "bigint", nullable: false),
                    MonthlyBillingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_MonthlyBillings_MonthlyBillingId",
                        column: x => x.MonthlyBillingId,
                        principalTable: "MonthlyBillings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MoneyCurrency = table.Column<string>(type: "text", nullable: false),
                    ExpenseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PlanId",
                table: "Expenses",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_MonthlyBillingId",
                table: "Incomes",
                column: "MonthlyBillingId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_MonthlyBillingId",
                table: "Plans",
                column: "MonthlyBillingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "MonthlyBillings");
        }
    }
}

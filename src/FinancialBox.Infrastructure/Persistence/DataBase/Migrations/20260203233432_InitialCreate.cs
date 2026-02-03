using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialBox.Infrastructure.Persistence.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "varchar(255)", maxLength: 100, nullable: false),
                    Email_Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 150, nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IdealMonthlyContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CoverImagePath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialGoalTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    FinancialGoalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialGoalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialGoalTransactions_FinancialGoals_FinancialGoalId",
                        column: x => x.FinancialGoalId,
                        principalTable: "FinancialGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_UserId",
                table: "FinancialGoals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoalTransactions_FinancialGoalId",
                table: "FinancialGoalTransactions",
                column: "FinancialGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Address",
                table: "Users",
                column: "Email_Address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialGoalTransactions");

            migrationBuilder.DropTable(
                name: "FinancialGoals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

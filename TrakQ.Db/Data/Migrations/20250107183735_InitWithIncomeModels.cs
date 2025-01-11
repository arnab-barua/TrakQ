using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrakQ.Db.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitWithIncomeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IncomeHeads",
                columns: table => new
                {
                    IncomeHeadId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IncomeHeadName = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeHeads", x => x.IncomeHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    IncomeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IncomeHeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    IncomeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.IncomeId);
                    table.ForeignKey(
                        name: "FK_Incomes_IncomeHeads_IncomeHeadId",
                        column: x => x.IncomeHeadId,
                        principalTable: "IncomeHeads",
                        principalColumn: "IncomeHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeHeadId",
                table: "Incomes",
                column: "IncomeHeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "IncomeHeads");
        }
    }
}

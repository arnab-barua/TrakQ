using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrakQ.Db.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenditureHeads",
                columns: table => new
                {
                    ExpenditureHeadId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HeadName = table.Column<string>(type: "TEXT", nullable: false),
                    ParentHeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true),
                    FixedAmount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    Budget = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenditureHeads", x => x.ExpenditureHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Expenditures",
                columns: table => new
                {
                    ExpenditureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExpenditureHeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentHeadId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpenditureDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenditures", x => x.ExpenditureId);
                    table.ForeignKey(
                        name: "FK_Expenditures_ExpenditureHeads_ExpenditureHeadId",
                        column: x => x.ExpenditureHeadId,
                        principalTable: "ExpenditureHeads",
                        principalColumn: "ExpenditureHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_ExpenditureHeadId",
                table: "Expenditures",
                column: "ExpenditureHeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenditures");

            migrationBuilder.DropTable(
                name: "ExpenditureHeads");
        }
    }
}

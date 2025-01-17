using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrakQ.Db.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdddedAccountTablesAndFiscalMonth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "FiscalMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<short>(type: "INTEGER", nullable: false),
                    Month = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountSheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OpeningBalance = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    ClosingBalance = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    FiscalMonthId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountSheets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountSheets_FiscalMonths_FiscalMonthId",
                        column: x => x.FiscalMonthId,
                        principalTable: "FiscalMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountSheets_AccountId",
                table: "AccountSheets",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSheets_FiscalMonthId",
                table: "AccountSheets",
                column: "FiscalMonthId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSheets");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "FiscalMonths");
        }
    }
}

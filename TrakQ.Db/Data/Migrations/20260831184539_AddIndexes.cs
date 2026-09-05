using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrakQ.Db.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccountSheets_AccountId",
                table: "AccountSheets");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeDate",
                table: "Incomes",
                column: "IncomeDate");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IsDeleted",
                table: "Incomes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalMonths_Year_Month",
                table: "FiscalMonths",
                columns: new[] { "Year", "Month" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_ExpenditureDate",
                table: "Expenditures",
                column: "ExpenditureDate");

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_IsDeleted",
                table: "Expenditures",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_ParentHeadId",
                table: "Expenditures",
                column: "ParentHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSheets_AccountId_FiscalMonthId",
                table: "AccountSheets",
                columns: new[] { "AccountId", "FiscalMonthId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Incomes_IncomeDate",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_IsDeleted",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_FiscalMonths_Year_Month",
                table: "FiscalMonths");

            migrationBuilder.DropIndex(
                name: "IX_Expenditures_ExpenditureDate",
                table: "Expenditures");

            migrationBuilder.DropIndex(
                name: "IX_Expenditures_IsDeleted",
                table: "Expenditures");

            migrationBuilder.DropIndex(
                name: "IX_Expenditures_ParentHeadId",
                table: "Expenditures");

            migrationBuilder.DropIndex(
                name: "IX_AccountSheets_AccountId_FiscalMonthId",
                table: "AccountSheets");

            migrationBuilder.CreateIndex(
                name: "IX_AccountSheets_AccountId",
                table: "AccountSheets",
                column: "AccountId");
        }
    }
}

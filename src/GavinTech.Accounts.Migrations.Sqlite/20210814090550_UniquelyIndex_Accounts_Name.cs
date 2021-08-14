using Microsoft.EntityFrameworkCore.Migrations;

namespace GavinTech.Accounts.Migrations.Sqlite
{
    public partial class UniquelyIndex_Accounts_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name",
                table: "Accounts",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_Name",
                table: "Accounts");
        }
    }
}

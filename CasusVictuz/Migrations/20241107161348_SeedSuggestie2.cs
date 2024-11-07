using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class SeedSuggestie2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title" },
                values: new object[] { 420, "Suggestie" });
        }

    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasusVictuz.Migrations
{
    /// <inheritdoc />
    public partial class urllinkEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlLinkPicture",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlLinkPicture",
                table: "Events");
        }
    }
}

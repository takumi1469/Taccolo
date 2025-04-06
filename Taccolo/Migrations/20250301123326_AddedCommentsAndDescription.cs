using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommentsAndDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LearningSets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "LearningSets");
        }
    }
}

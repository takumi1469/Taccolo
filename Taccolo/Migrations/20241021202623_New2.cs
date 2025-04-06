using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class New2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForMigration",
                table: "LearningSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForMigration",
                table: "LearningSets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

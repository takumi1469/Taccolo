using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class AddedDateToCommentAndHelpReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "HelpReplys",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Comments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "HelpReplys");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Comments");
        }
    }
}

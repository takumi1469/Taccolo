using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class CommentedOut2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningSets_AspNetUsers_UserId",
                table: "LearningSets");

            migrationBuilder.DropIndex(
                name: "IX_LearningSets_UserId",
                table: "LearningSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LearningSets_UserId",
                table: "LearningSets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningSets_AspNetUsers_UserId",
                table: "LearningSets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

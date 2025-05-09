using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class AddedSlugToUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicSlug",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PublicSlug",
                table: "AspNetUsers",
                column: "PublicSlug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PublicSlug",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicSlug",
                table: "AspNetUsers");
        }
    }
}

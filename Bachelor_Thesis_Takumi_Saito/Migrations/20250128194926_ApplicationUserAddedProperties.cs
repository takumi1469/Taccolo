using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelor_Thesis_Takumi_Saito.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserAddedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "DesiredLanguages",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "KnownLanguages",
                table: "AspNetUsers",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DesiredLanguages",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KnownLanguages",
                table: "AspNetUsers");
        }
    }
}

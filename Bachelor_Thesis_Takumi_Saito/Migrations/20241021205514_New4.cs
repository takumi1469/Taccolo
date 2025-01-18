using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelor_Thesis_Takumi_Saito.Migrations
{
    /// <inheritdoc />
    public partial class New4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LearningSets",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LearningSets");
        }
    }
}

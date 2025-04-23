using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class Uncommented : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WordMeaningPairs_LsId",
                table: "WordMeaningPairs",
                column: "LsId");

            migrationBuilder.AddForeignKey(
                name: "FK_WordMeaningPairs_LearningSets_LsId",
                table: "WordMeaningPairs",
                column: "LsId",
                principalTable: "LearningSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordMeaningPairs_LearningSets_LsId",
                table: "WordMeaningPairs");

            migrationBuilder.DropIndex(
                name: "IX_WordMeaningPairs_LsId",
                table: "WordMeaningPairs");
        }
    }
}

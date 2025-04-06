using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class WordMeaningPairsAddedToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordMeaningPairs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LsId = table.Column<Guid>(type: "uuid", nullable: false),
                    Word = table.Column<string>(type: "text", nullable: false),
                    TranslatedText = table.Column<string>(type: "text", nullable: false),
                    Alternatives = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordMeaningPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordMeaningPairs_LearningSets_LsId",
                        column: x => x.LsId,
                        principalTable: "LearningSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordMeaningPairs_LsId",
                table: "WordMeaningPairs",
                column: "LsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordMeaningPairs");
        }
    }
}

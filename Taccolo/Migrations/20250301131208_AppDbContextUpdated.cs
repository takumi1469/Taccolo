using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taccolo.Migrations
{
    /// <inheritdoc />
    public partial class AppDbContextUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    LsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_LearningSets_LsId",
                        column: x => x.LsId,
                        principalTable: "LearningSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HelpRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    LsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpRequests_LearningSets_LsId",
                        column: x => x.LsId,
                        principalTable: "LearningSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HelpReplys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpReplys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpReplys_HelpRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "HelpRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LsId",
                table: "Comments",
                column: "LsId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpReplys_RequestId",
                table: "HelpReplys",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpRequests_LsId",
                table: "HelpRequests",
                column: "LsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "HelpReplys");

            migrationBuilder.DropTable(
                name: "HelpRequests");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelor_Thesis_Takumi_Saito.Migrations
{
    /// <inheritdoc />
    public partial class WMP_Added_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WordMeaningPairs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "WordMeaningPairs");
        }
    }
}

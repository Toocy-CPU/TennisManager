using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisManager.Migrations
{
    /// <inheritdoc />
    public partial class AddBracketNavigationToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextMatchId",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextMatchSlot",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_NextMatchId",
                table: "Matches",
                column: "NextMatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Matches_NextMatchId",
                table: "Matches",
                column: "NextMatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Matches_NextMatchId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_NextMatchId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "NextMatchId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "NextMatchSlot",
                table: "Matches");
        }
    }
}

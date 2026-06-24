using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedProgramIdToShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Shows_ProgramId",
                table: "Shows",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Programs_ProgramId",
                table: "Shows",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Programs_ProgramId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ProgramId",
                table: "Shows");
        }
    }
}

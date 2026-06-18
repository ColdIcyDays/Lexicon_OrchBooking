using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamedVenueClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrice_Venues_VenueId",
                table: "TicketPrice");

            migrationBuilder.RenameColumn(
                name: "VenueId",
                table: "TicketPrice",
                newName: "OrchVenueId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPrice_VenueId",
                table: "TicketPrice",
                newName: "IX_TicketPrice_OrchVenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrice_Venues_OrchVenueId",
                table: "TicketPrice",
                column: "OrchVenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrice_Venues_OrchVenueId",
                table: "TicketPrice");

            migrationBuilder.RenameColumn(
                name: "OrchVenueId",
                table: "TicketPrice",
                newName: "VenueId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketPrice_OrchVenueId",
                table: "TicketPrice",
                newName: "IX_TicketPrice_VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrice_Venues_VenueId",
                table: "TicketPrice",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}

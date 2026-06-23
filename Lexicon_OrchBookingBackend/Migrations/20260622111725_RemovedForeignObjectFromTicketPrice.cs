using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemovedForeignObjectFromTicketPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_Venues_VenueId",
                table: "TicketPrices");

            migrationBuilder.DropIndex(
                name: "IX_TicketPrices_VenueId",
                table: "TicketPrices");

            migrationBuilder.AddColumn<int>(
                name: "OrchVenueId",
                table: "TicketPrices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrices_OrchVenueId",
                table: "TicketPrices",
                column: "OrchVenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices",
                column: "OrchVenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices");

            migrationBuilder.DropIndex(
                name: "IX_TicketPrices_OrchVenueId",
                table: "TicketPrices");

            migrationBuilder.DropColumn(
                name: "OrchVenueId",
                table: "TicketPrices");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrices_VenueId",
                table: "TicketPrices",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_Venues_VenueId",
                table: "TicketPrices",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

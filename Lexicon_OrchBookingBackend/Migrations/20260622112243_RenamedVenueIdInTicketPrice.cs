using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenamedVenueIdInTicketPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "TicketPrices");

            migrationBuilder.AlterColumn<int>(
                name: "OrchVenueId",
                table: "TicketPrices",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices",
                column: "OrchVenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices");

            migrationBuilder.AlterColumn<int>(
                name: "OrchVenueId",
                table: "TicketPrices",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "TicketPrices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_Venues_OrchVenueId",
                table: "TicketPrices",
                column: "OrchVenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}

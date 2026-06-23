using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedVenueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserDatas_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_TicketPrice_PriceId",
                table: "PurchasedTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrice_Venues_OrchVenueId",
                table: "TicketPrice");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPrice",
                table: "TicketPrice");

            migrationBuilder.DropIndex(
                name: "IX_TicketPrice_OrchVenueId",
                table: "TicketPrice");

            migrationBuilder.DropColumn(
                name: "OrchVenueId",
                table: "TicketPrice");

            migrationBuilder.RenameTable(
                name: "TicketPrice",
                newName: "TicketPrices");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "TicketPrices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPrices",
                table: "TicketPrices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrices_VenueId",
                table: "TicketPrices",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_TicketPrices_PriceId",
                table: "PurchasedTickets",
                column: "PriceId",
                principalTable: "TicketPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrices_Venues_VenueId",
                table: "TicketPrices",
                column: "VenueId",
                principalTable: "Venues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_TicketPrices_PriceId",
                table: "PurchasedTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketPrices_Venues_VenueId",
                table: "TicketPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketPrices",
                table: "TicketPrices");

            migrationBuilder.DropIndex(
                name: "IX_TicketPrices_VenueId",
                table: "TicketPrices");

            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "TicketPrices");

            migrationBuilder.RenameTable(
                name: "TicketPrices",
                newName: "TicketPrice");

            migrationBuilder.AddColumn<int>(
                name: "OrchVenueId",
                table: "TicketPrice",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketPrice",
                table: "TicketPrice",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserDataId",
                table: "AspNetUsers",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrice_OrchVenueId",
                table: "TicketPrice",
                column: "OrchVenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserDatas_UserDataId",
                table: "AspNetUsers",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_TicketPrice_PriceId",
                table: "PurchasedTickets",
                column: "PriceId",
                principalTable: "TicketPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketPrice_Venues_OrchVenueId",
                table: "TicketPrice",
                column: "OrchVenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }
    }
}

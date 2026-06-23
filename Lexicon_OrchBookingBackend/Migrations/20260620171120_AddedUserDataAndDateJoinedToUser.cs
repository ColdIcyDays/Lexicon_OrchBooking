using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserDataAndDateJoinedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userData",
                table: "AspNetUsers",
                newName: "UserDataId");

            migrationBuilder.AddColumn<int>(
                name: "UserDataId",
                table: "PurchasedTickets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    DateJoined = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedTickets_UserDataId",
                table: "PurchasedTickets",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserDataId",
                table: "AspNetUsers",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserData_UserDataId",
                table: "AspNetUsers",
                column: "UserDataId",
                principalTable: "UserData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_UserData_UserDataId",
                table: "PurchasedTickets",
                column: "UserDataId",
                principalTable: "UserData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserData_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_UserData_UserDataId",
                table: "PurchasedTickets");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedTickets_UserDataId",
                table: "PurchasedTickets");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "PurchasedTickets");

            migrationBuilder.RenameColumn(
                name: "UserDataId",
                table: "AspNetUsers",
                newName: "userData");
        }
    }
}

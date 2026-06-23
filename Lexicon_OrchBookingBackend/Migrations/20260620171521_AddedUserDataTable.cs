using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_OrchBookingBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserData_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_UserData_UserDataId",
                table: "PurchasedTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserData",
                table: "UserData");

            migrationBuilder.RenameTable(
                name: "UserData",
                newName: "UserDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserDatas_UserDataId",
                table: "AspNetUsers",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedTickets_UserDatas_UserDataId",
                table: "PurchasedTickets",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserDatas_UserDataId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedTickets_UserDatas_UserDataId",
                table: "PurchasedTickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas");

            migrationBuilder.RenameTable(
                name: "UserDatas",
                newName: "UserData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserData",
                table: "UserData",
                column: "Id");

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
    }
}

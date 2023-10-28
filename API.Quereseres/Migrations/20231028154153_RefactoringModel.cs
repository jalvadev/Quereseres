using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Quereseres.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houseworks_Users_UserId",
                table: "Houseworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Houses_HomeId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Houses_HomeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HomeId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "HomeId",
                table: "Users",
                newName: "HouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HomeId",
                table: "Users",
                newName: "IX_Users_HouseId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Houseworks",
                newName: "AssignedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Houseworks_UserId",
                table: "Houseworks",
                newName: "IX_Houseworks_AssignedUserId");

            migrationBuilder.AddColumn<int>(
                name: "HouseId",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HouseId",
                table: "Rooms",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houseworks_Users_AssignedUserId",
                table: "Houseworks",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Houses_HouseId",
                table: "Rooms",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Houses_HouseId",
                table: "Users",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houseworks_Users_AssignedUserId",
                table: "Houseworks");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Houses_HouseId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Houses_HouseId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_HouseId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "HouseId",
                table: "Users",
                newName: "HomeId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HouseId",
                table: "Users",
                newName: "IX_Users_HomeId");

            migrationBuilder.RenameColumn(
                name: "AssignedUserId",
                table: "Houseworks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Houseworks_AssignedUserId",
                table: "Houseworks",
                newName: "IX_Houseworks_UserId");

            migrationBuilder.AddColumn<int>(
                name: "HomeId",
                table: "Rooms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HomeId",
                table: "Rooms",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houseworks_Users_UserId",
                table: "Houseworks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Houses_HomeId",
                table: "Rooms",
                column: "HomeId",
                principalTable: "Houses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Houses_HomeId",
                table: "Users",
                column: "HomeId",
                principalTable: "Houses",
                principalColumn: "Id");
        }
    }
}

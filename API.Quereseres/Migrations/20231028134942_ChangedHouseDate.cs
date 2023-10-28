using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Quereseres.Migrations
{
    /// <inheritdoc />
    public partial class ChangedHouseDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordInitDate",
                table: "Houses");

            migrationBuilder.AddColumn<int>(
                name: "LimitDay",
                table: "Houses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitDay",
                table: "Houses");

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordInitDate",
                table: "Houses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

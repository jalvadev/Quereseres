using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Quereseres.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseworkWeeklyToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HouseworkWeeklies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HouseworkId = table.Column<int>(type: "integer", nullable: false),
                    LimitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseworkWeeklies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseworkWeeklies_Houseworks_HouseworkId",
                        column: x => x.HouseworkId,
                        principalTable: "Houseworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseworkWeeklies_HouseworkId",
                table: "HouseworkWeeklies",
                column: "HouseworkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseworkWeeklies");
        }
    }
}

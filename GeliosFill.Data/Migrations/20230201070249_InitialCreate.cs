using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeliosFill.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFillInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFillInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelFillHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    DateOfFill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserFillInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFillHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFillHistories_UserFillInfos_UserFillInfoId",
                        column: x => x.UserFillInfoId,
                        principalTable: "UserFillInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelFillHistories_UserFillInfoId",
                table: "FuelFillHistories",
                column: "UserFillInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelFillHistories");

            migrationBuilder.DropTable(
                name: "UserFillInfos");
        }
    }
}

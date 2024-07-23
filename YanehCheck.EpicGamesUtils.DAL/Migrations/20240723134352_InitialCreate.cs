using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YanehCheck.EpicGamesUtils.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FortniteId = table.Column<string>(type: "TEXT", nullable: false),
                    FortniteGgId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    PriceVbucks = table.Column<int>(type: "INTEGER", nullable: true),
                    PriceUsd = table.Column<decimal>(type: "TEXT", nullable: true),
                    Season = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Set = table.Column<string>(type: "TEXT", nullable: true),
                    Release = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSeen = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Styles = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemEntities_FortniteGgId",
                table: "ItemEntities",
                column: "FortniteGgId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemEntities");
        }
    }
}

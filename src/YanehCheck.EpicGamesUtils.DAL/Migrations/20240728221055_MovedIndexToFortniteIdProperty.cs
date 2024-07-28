using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YanehCheck.EpicGamesUtils.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MovedIndexToFortniteIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemEntities_FortniteGgId",
                table: "ItemEntities");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEntities_FortniteId",
                table: "ItemEntities",
                column: "FortniteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemEntities_FortniteId",
                table: "ItemEntities");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEntities_FortniteGgId",
                table: "ItemEntities",
                column: "FortniteGgId");
        }
    }
}

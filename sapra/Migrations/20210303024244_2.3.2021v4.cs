using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class _232021v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_MapLayerField_MapLayerId",
                table: "MapLayerField",
                column: "MapLayerId",
                principalTable: "MapLayer",
                principalColumn: "MapLayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayerField_MapLayerId",
                table: "MapLayerField");
        }
    }
}

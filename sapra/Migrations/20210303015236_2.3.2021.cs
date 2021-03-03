using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class _232021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayerRepository",
                table: "MapLayerRepository");

            migrationBuilder.RenameTable(
                name: "MapLayerRepository",
                newName: "MapLayer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayer",
                table: "MapLayer",
                column: "MapLayerId");

            migrationBuilder.CreateTable(
                name: "MapLayerField",
                columns: table => new
                {
                    MapLayerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapLayerField", x => new { x.MapLayerId, x.Name });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MapLayerField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayer",
                table: "MapLayer");

            migrationBuilder.RenameTable(
                name: "MapLayer",
                newName: "MapLayerRepository");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayerRepository",
                table: "MapLayerRepository",
                column: "MapLayerId");
        }
    }
}

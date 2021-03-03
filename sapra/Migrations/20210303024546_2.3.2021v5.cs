using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class _232021v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayerField_MapLayerId",
                table: "MapLayerField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayerField",
                table: "MapLayerField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayer",
                table: "MapLayer");

            migrationBuilder.RenameTable(
                name: "MapLayerField",
                newName: "MapLayerFieldRepository");

            migrationBuilder.RenameTable(
                name: "MapLayer",
                newName: "MapLayerRepository");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayerFieldRepository",
                table: "MapLayerFieldRepository",
                columns: new[] { "MapLayerId", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayerRepository",
                table: "MapLayerRepository",
                column: "MapLayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayerFieldRepository_MapLayerRepository_MapLayerId",
                table: "MapLayerFieldRepository",
                column: "MapLayerId",
                principalTable: "MapLayerRepository",
                principalColumn: "MapLayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapLayerFieldRepository_MapLayerRepository_MapLayerId",
                table: "MapLayerFieldRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayerRepository",
                table: "MapLayerRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapLayerFieldRepository",
                table: "MapLayerFieldRepository");

            migrationBuilder.RenameTable(
                name: "MapLayerRepository",
                newName: "MapLayer");

            migrationBuilder.RenameTable(
                name: "MapLayerFieldRepository",
                newName: "MapLayerField");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayer",
                table: "MapLayer",
                column: "MapLayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapLayerField",
                table: "MapLayerField",
                columns: new[] { "MapLayerId", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_MapLayerField_MapLayerId",
                table: "MapLayerField",
                column: "MapLayerId",
                principalTable: "MapLayer",
                principalColumn: "MapLayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

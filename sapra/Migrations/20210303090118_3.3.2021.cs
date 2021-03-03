using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class _332021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Visible",
                table: "MapLayerRepository",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visible",
                table: "MapLayerRepository");
        }
    }
}

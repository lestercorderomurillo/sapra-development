using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class Update121120v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_RoleRepository_RoleId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission");

            migrationBuilder.RenameTable(
                name: "RolePermission",
                newName: "RolePermissionRepository");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissionRepository_RoleRepository_RoleId",
                table: "RolePermissionRepository",
                column: "RoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissionRepository_RoleRepository_RoleId",
                table: "RolePermissionRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository");

            migrationBuilder.RenameTable(
                name: "RolePermissionRepository",
                newName: "RolePermission");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_RoleRepository_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

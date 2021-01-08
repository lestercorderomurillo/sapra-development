using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class PermissionString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "RolePermissionRepository");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoleRepository",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "PermissionName",
                table: "RolePermissionRepository",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository",
                columns: new[] { "RoleId", "PermissionName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository");

            migrationBuilder.DropColumn(
                name: "PermissionName",
                table: "RolePermissionRepository");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoleRepository",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "RolePermissionRepository",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissionRepository",
                table: "RolePermissionRepository",
                columns: new[] { "RoleId", "PermissionId" });
        }
    }
}

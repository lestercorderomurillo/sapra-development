using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class Update121120v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleRepository_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository");

            migrationBuilder.DropIndex(
                name: "IX_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository");

            migrationBuilder.DropColumn(
                name: "ParentRoleRoleId",
                table: "RoleRepository");

            migrationBuilder.AddColumn<int>(
                name: "ParentRoleId",
                table: "RoleRepository",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentRoleId",
                table: "RoleRepository");

            migrationBuilder.AddColumn<int>(
                name: "ParentRoleRoleId",
                table: "RoleRepository",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository",
                column: "ParentRoleRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRepository_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository",
                column: "ParentRoleRoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

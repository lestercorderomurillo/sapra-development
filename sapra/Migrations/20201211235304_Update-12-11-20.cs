using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class Update121120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Role_ParentRoleRoleId",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfo_User_UserId",
                table: "UserInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPhone_UserInfo_UserId",
                table: "UserPhone");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPhone",
                table: "UserPhone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfo",
                table: "UserInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "UserPhone",
                newName: "UserPhoneRepository");

            migrationBuilder.RenameTable(
                name: "UserInfo",
                newName: "UserInfoRepository");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "UserRepository");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "RoleRepository");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "UserRepository",
                newName: "IX_UserRepository_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Role_ParentRoleRoleId",
                table: "RoleRepository",
                newName: "IX_RoleRepository_ParentRoleRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPhoneRepository",
                table: "UserPhoneRepository",
                columns: new[] { "UserId", "Number" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfoRepository",
                table: "UserInfoRepository",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRepository",
                table: "UserRepository",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleRepository",
                table: "RoleRepository",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_RoleRepository_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleRepository_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository",
                column: "ParentRoleRoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfoRepository_UserRepository_UserId",
                table: "UserInfoRepository",
                column: "UserId",
                principalTable: "UserRepository",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhoneRepository_UserInfoRepository_UserId",
                table: "UserPhoneRepository",
                column: "UserId",
                principalTable: "UserInfoRepository",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRepository_RoleRepository_RoleId",
                table: "UserRepository",
                column: "RoleId",
                principalTable: "RoleRepository",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_RoleRepository_RoleId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleRepository_RoleRepository_ParentRoleRoleId",
                table: "RoleRepository");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfoRepository_UserRepository_UserId",
                table: "UserInfoRepository");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPhoneRepository_UserInfoRepository_UserId",
                table: "UserPhoneRepository");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRepository_RoleRepository_RoleId",
                table: "UserRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRepository",
                table: "UserRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPhoneRepository",
                table: "UserPhoneRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfoRepository",
                table: "UserInfoRepository");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleRepository",
                table: "RoleRepository");

            migrationBuilder.RenameTable(
                name: "UserRepository",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "UserPhoneRepository",
                newName: "UserPhone");

            migrationBuilder.RenameTable(
                name: "UserInfoRepository",
                newName: "UserInfo");

            migrationBuilder.RenameTable(
                name: "RoleRepository",
                newName: "Role");

            migrationBuilder.RenameIndex(
                name: "IX_UserRepository_RoleId",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleRepository_ParentRoleRoleId",
                table: "Role",
                newName: "IX_Role_ParentRoleRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPhone",
                table: "UserPhone",
                columns: new[] { "UserId", "Number" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfo",
                table: "UserInfo",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "RoleId");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Role_ParentRoleRoleId",
                table: "Role",
                column: "ParentRoleRoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_User_UserId",
                table: "UserInfo",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhone_UserInfo_UserId",
                table: "UserPhone",
                column: "UserId",
                principalTable: "UserInfo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

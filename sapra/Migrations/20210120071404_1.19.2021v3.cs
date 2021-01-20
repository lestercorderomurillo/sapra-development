using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sapra.Migrations
{
    public partial class _1192021v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginAttemptRecoveryTimestamp",
                table: "UserRepository");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LoginAttemptRecoveryTimestamp",
                table: "UserRepository",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

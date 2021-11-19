using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_UserId1",
                table: "users",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_users_AspNetUsers_UserId1",
                table: "users",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_AspNetUsers_UserId1",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_UserId1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "users",
                type: "nvarchar(450)",
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
    }
}

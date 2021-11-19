using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_users_ID",
                table: "users",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_users_AspNetUsers_ID",
                table: "users",
                column: "ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_AspNetUsers_ID",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_ID",
                table: "users");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "users");
        }
    }
}

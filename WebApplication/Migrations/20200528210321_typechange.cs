using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication.Migrations
{
    public partial class typechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Type",
                table: "Test",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true,
                defaultValue:0);

            migrationBuilder.AlterColumn<bool>(
                name: "ValidationStatus",
                table: "Doctor",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Test",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(short),
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<bool>(
                name: "ValidationStatus",
                table: "Doctor",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}

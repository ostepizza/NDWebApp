using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NDWebApp.Migrations
{
    public partial class newfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "empFname",
                table: "AspNetUsers",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "empLname",
                table: "AspNetUsers",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "empNr",
                table: "AspNetUsers",
                type: "int",
                maxLength: 250,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "empFname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "empLname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "empNr",
                table: "AspNetUsers");
        }
    }
}

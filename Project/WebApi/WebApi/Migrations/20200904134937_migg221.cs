using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class migg221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCarRated",
                table: "CarRents");

            migrationBuilder.DropColumn(
                name: "IsRACSRated",
                table: "CarRents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCarRated",
                table: "CarRents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRACSRated",
                table: "CarRents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

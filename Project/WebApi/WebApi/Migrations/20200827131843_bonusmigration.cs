using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class bonusmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BonusPoints",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bonus",
                columns: table => new
                {
                    BonusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusPerKilometer = table.Column<int>(nullable: false),
                    DiscountPerReservation = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonus", x => x.BonusId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonus");

            migrationBuilder.DropColumn(
                name: "BonusPoints",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}

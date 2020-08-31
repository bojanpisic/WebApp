using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class mig123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "FlightReservations",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "FlightRates",
                columns: table => new
                {
                    FlightRateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<float>(nullable: false),
                    FlightId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightRates", x => x.FlightRateId);
                    table.ForeignKey(
                        name: "FK_FlightRates_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightRates_FlightId",
                table: "FlightRates",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightRates_UserId",
                table: "FlightRates",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightRates");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "FlightReservations");
        }
    }
}

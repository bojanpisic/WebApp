using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class newTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "ReservationFlightReservationId",
                table: "Tickets2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationFlightReservationId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlightReservations",
                columns: table => new
                {
                    FlightReservationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightReservations", x => x.FlightReservationId);
                    table.ForeignKey(
                        name: "FK_FlightReservations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets2_ReservationFlightReservationId",
                table: "Tickets2",
                column: "ReservationFlightReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReservationFlightReservationId",
                table: "Tickets",
                column: "ReservationFlightReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightReservations_UserId",
                table: "FlightReservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_FlightReservations_ReservationFlightReservationId",
                table: "Tickets",
                column: "ReservationFlightReservationId",
                principalTable: "FlightReservations",
                principalColumn: "FlightReservationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets2_FlightReservations_ReservationFlightReservationId",
                table: "Tickets2",
                column: "ReservationFlightReservationId",
                principalTable: "FlightReservations",
                principalColumn: "FlightReservationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_FlightReservations_ReservationFlightReservationId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets2_FlightReservations_ReservationFlightReservationId",
                table: "Tickets2");

            migrationBuilder.DropTable(
                name: "FlightReservations");

            migrationBuilder.DropIndex(
                name: "IX_Tickets2_ReservationFlightReservationId",
                table: "Tickets2");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReservationFlightReservationId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReservationFlightReservationId",
                table: "Tickets2");

            migrationBuilder.DropColumn(
                name: "ReservationFlightReservationId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

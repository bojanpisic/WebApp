using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class reservationChangedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Expires",
                table: "Invitations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CarRentId",
                table: "FlightReservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlightReservations_CarRentId",
                table: "FlightReservations",
                column: "CarRentId",
                unique: true,
                filter: "[CarRentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightReservations_CarRents_CarRentId",
                table: "FlightReservations",
                column: "CarRentId",
                principalTable: "CarRents",
                principalColumn: "CarRentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightReservations_CarRents_CarRentId",
                table: "FlightReservations");

            migrationBuilder.DropIndex(
                name: "IX_FlightReservations_CarRentId",
                table: "FlightReservations");

            migrationBuilder.DropColumn(
                name: "Expires",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CarRentId",
                table: "FlightReservations");
        }
    }
}

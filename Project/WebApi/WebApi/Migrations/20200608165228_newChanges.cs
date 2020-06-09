using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class newChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Airlines_AirlineId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_CityStateAddresses_AddressId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_CityStateAddresses_FromCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_CityStateAddresses_ToCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightsAddresses_CityStateAddresses_CityStateAddressId",
                table: "FlightsAddresses");

            migrationBuilder.DropTable(
                name: "CityStateAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FromCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ToCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_AddressId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AirlineId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CityStateAddressId",
                table: "FlightsAddresses");

            migrationBuilder.DropColumn(
                name: "FromCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "LandingDate",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "LandingTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TakeOffDate",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TakeOffTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ToCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "AirlineId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                table: "FlightsAddresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromDestinationId",
                table: "Flights",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LandingDateTime",
                table: "Flights",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TakeOffDateTime",
                table: "Flights",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ToDestinationId",
                table: "Flights",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Destinations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Destinations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Airlines",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses",
                columns: new[] { "DestinationId", "FlightId" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FromDestinationId",
                table: "Flights",
                column: "FromDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ToDestinationId",
                table: "Flights",
                column: "ToDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Airlines_AdminId",
                table: "Airlines",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Airlines_AspNetUsers_AdminId",
                table: "Airlines",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_FromDestinationId",
                table: "Flights",
                column: "FromDestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Destinations_ToDestinationId",
                table: "Flights",
                column: "ToDestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightsAddresses_Destinations_DestinationId",
                table: "FlightsAddresses",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Airlines_AspNetUsers_AdminId",
                table: "Airlines");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_FromDestinationId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Destinations_ToDestinationId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightsAddresses_Destinations_DestinationId",
                table: "FlightsAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FromDestinationId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ToDestinationId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Airlines_AdminId",
                table: "Airlines");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "FlightsAddresses");

            migrationBuilder.DropColumn(
                name: "FromDestinationId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "LandingDateTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "TakeOffDateTime",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ToDestinationId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Airlines");

            migrationBuilder.AddColumn<int>(
                name: "CityStateAddressId",
                table: "FlightsAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromCityStateAddressId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LandingDate",
                table: "Flights",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LandingTime",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TakeOffDate",
                table: "Flights",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TakeOffTime",
                table: "Flights",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToCityStateAddressId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Destinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AirlineId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses",
                columns: new[] { "CityStateAddressId", "FlightId" });

            migrationBuilder.CreateTable(
                name: "CityStateAddresses",
                columns: table => new
                {
                    CityStateAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityStateAddresses", x => x.CityStateAddressId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FromCityStateAddressId",
                table: "Flights",
                column: "FromCityStateAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ToCityStateAddressId",
                table: "Flights",
                column: "ToCityStateAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_AddressId",
                table: "Destinations",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AirlineId",
                table: "AspNetUsers",
                column: "AirlineId",
                unique: true,
                filter: "[AirlineId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Airlines_AirlineId",
                table: "AspNetUsers",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_CityStateAddresses_AddressId",
                table: "Destinations",
                column: "AddressId",
                principalTable: "CityStateAddresses",
                principalColumn: "CityStateAddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_CityStateAddresses_FromCityStateAddressId",
                table: "Flights",
                column: "FromCityStateAddressId",
                principalTable: "CityStateAddresses",
                principalColumn: "CityStateAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_CityStateAddresses_ToCityStateAddressId",
                table: "Flights",
                column: "ToCityStateAddressId",
                principalTable: "CityStateAddresses",
                principalColumn: "CityStateAddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightsAddresses_CityStateAddresses_CityStateAddressId",
                table: "FlightsAddresses",
                column: "CityStateAddressId",
                principalTable: "CityStateAddresses",
                principalColumn: "CityStateAddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

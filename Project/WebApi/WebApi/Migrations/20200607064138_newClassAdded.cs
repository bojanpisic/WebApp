using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class newClassAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Addresses_AddressId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Addresses_FromAddressId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Addresses_ToAddressId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightsAddresses_Addresses_AddressId",
                table: "FlightsAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FromAddressId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_ToAddressId",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "FlightsAddresses");

            migrationBuilder.DropColumn(
                name: "FromAddressId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ToAddressId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CityStateAddressId",
                table: "FlightsAddresses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromCityStateAddressId",
                table: "Flights",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToCityStateAddressId",
                table: "Flights",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airlines",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses",
                columns: new[] { "CityStateAddressId", "FlightId" });

            migrationBuilder.CreateTable(
                name: "CityStateAddresses",
                columns: table => new
                {
                    CityStateAddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "CityStateAddressId",
                table: "FlightsAddresses");

            migrationBuilder.DropColumn(
                name: "FromCityStateAddressId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "ToCityStateAddressId",
                table: "Flights");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "FlightsAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromAddressId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToAddressId",
                table: "Flights",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airlines",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightsAddresses",
                table: "FlightsAddresses",
                columns: new[] { "AddressId", "FlightId" });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FromAddressId",
                table: "Flights",
                column: "FromAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ToAddressId",
                table: "Flights",
                column: "ToAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Addresses_AddressId",
                table: "Destinations",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Addresses_FromAddressId",
                table: "Flights",
                column: "FromAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Addresses_ToAddressId",
                table: "Flights",
                column: "ToAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightsAddresses_Addresses_AddressId",
                table: "FlightsAddresses",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

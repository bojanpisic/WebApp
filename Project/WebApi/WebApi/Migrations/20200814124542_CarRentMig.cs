using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class CarRentMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirlineRates_Airlines_AirlineId",
                table: "AirlineRates");

            migrationBuilder.DropForeignKey(
                name: "FK_AirlineRates_AspNetUsers_UserId",
                table: "AirlineRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RentCarServiceRates_RentACarServices_RentACarServiceId",
                table: "RentCarServiceRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RentCarServiceRates_AspNetUsers_UserId",
                table: "RentCarServiceRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentCarServiceRates",
                table: "RentCarServiceRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirlineRates",
                table: "AirlineRates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RentCarServiceRates",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "RentACarServiceId",
                table: "RentCarServiceRates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RentCarServiceRatesId",
                table: "RentCarServiceRates",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "AirlineRates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AirlineRates",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "AirlineRateId",
                table: "AirlineRates",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentCarServiceRates",
                table: "RentCarServiceRates",
                column: "RentCarServiceRatesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirlineRates",
                table: "AirlineRates",
                column: "AirlineRateId");

            migrationBuilder.CreateTable(
                name: "CarRates",
                columns: table => new
                {
                    CarRateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<float>(nullable: false),
                    CarId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRates", x => x.CarRateId);
                    table.ForeignKey(
                        name: "FK_CarRates_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarRents",
                columns: table => new
                {
                    CarRentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TakeOverDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    TakeOverCity = table.Column<string>(nullable: true),
                    ReturnCity = table.Column<string>(nullable: true),
                    RentedCarCarId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRents", x => x.CarRentId);
                    table.ForeignKey(
                        name: "FK_CarRents_Cars_RentedCarCarId",
                        column: x => x.RentedCarCarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarRents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentCarServiceRates_RentACarServiceId",
                table: "RentCarServiceRates",
                column: "RentACarServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AirlineRates_UserId",
                table: "AirlineRates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRates_CarId",
                table: "CarRates",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRates_UserId",
                table: "CarRates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRents_RentedCarCarId",
                table: "CarRents",
                column: "RentedCarCarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRents_UserId",
                table: "CarRents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirlineRates_Airlines_AirlineId",
                table: "AirlineRates",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AirlineRates_AspNetUsers_UserId",
                table: "AirlineRates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentCarServiceRates_RentACarServices_RentACarServiceId",
                table: "RentCarServiceRates",
                column: "RentACarServiceId",
                principalTable: "RentACarServices",
                principalColumn: "RentACarServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RentCarServiceRates_AspNetUsers_UserId",
                table: "RentCarServiceRates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirlineRates_Airlines_AirlineId",
                table: "AirlineRates");

            migrationBuilder.DropForeignKey(
                name: "FK_AirlineRates_AspNetUsers_UserId",
                table: "AirlineRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RentCarServiceRates_RentACarServices_RentACarServiceId",
                table: "RentCarServiceRates");

            migrationBuilder.DropForeignKey(
                name: "FK_RentCarServiceRates_AspNetUsers_UserId",
                table: "RentCarServiceRates");

            migrationBuilder.DropTable(
                name: "CarRates");

            migrationBuilder.DropTable(
                name: "CarRents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RentCarServiceRates",
                table: "RentCarServiceRates");

            migrationBuilder.DropIndex(
                name: "IX_RentCarServiceRates_RentACarServiceId",
                table: "RentCarServiceRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirlineRates",
                table: "AirlineRates");

            migrationBuilder.DropIndex(
                name: "IX_AirlineRates_UserId",
                table: "AirlineRates");

            migrationBuilder.DropColumn(
                name: "RentCarServiceRatesId",
                table: "RentCarServiceRates");

            migrationBuilder.DropColumn(
                name: "AirlineRateId",
                table: "AirlineRates");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RentCarServiceRates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RentACarServiceId",
                table: "RentCarServiceRates",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AirlineRates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirlineId",
                table: "AirlineRates",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RentCarServiceRates",
                table: "RentCarServiceRates",
                columns: new[] { "RentACarServiceId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirlineRates",
                table: "AirlineRates",
                columns: new[] { "UserId", "AirlineId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AirlineRates_Airlines_AirlineId",
                table: "AirlineRates",
                column: "AirlineId",
                principalTable: "Airlines",
                principalColumn: "AirlineId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AirlineRates_AspNetUsers_UserId",
                table: "AirlineRates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentCarServiceRates_RentACarServices_RentACarServiceId",
                table: "RentCarServiceRates",
                column: "RentACarServiceId",
                principalTable: "RentACarServices",
                principalColumn: "RentACarServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentCarServiceRates_AspNetUsers_UserId",
                table: "RentCarServiceRates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Address2_Address2Id",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_Address2Id",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Address2Id",
                table: "Branches");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Branches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Branches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Branches");

            migrationBuilder.AddColumn<int>(
                name: "Address2Id",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Address2Id",
                table: "Branches",
                column: "Address2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Address2_Address2Id",
                table: "Branches",
                column: "Address2Id",
                principalTable: "Address2",
                principalColumn: "Address2Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

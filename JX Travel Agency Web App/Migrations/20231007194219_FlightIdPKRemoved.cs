using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JX_Travel_Agency_Web_App.Migrations
{
    public partial class FlightIdPKRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatInventories",
                table: "SeatInventories");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "SeatInventories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatInventories",
                table: "SeatInventories",
                columns: new[] { "FlightId", "Class" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeatInventories",
                table: "SeatInventories");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "SeatInventories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeatInventories",
                table: "SeatInventories",
                column: "FlightId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JX_Travel_Agency_Web_App.Migrations
{
    public partial class initialRE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    AirlineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AircraftType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureAirportCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArrivalAirportCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_Airports_ArrivalAirportCode",
                        column: x => x.ArrivalAirportCode,
                        principalTable: "Airports",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Flights_Airports_DepartureAirportCode",
                        column: x => x.DepartureAirportCode,
                        principalTable: "Airports",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatInventories",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeatCapacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatAvailable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatInventories", x => new { x.FlightId, x.Class });
                    table.ForeignKey(
                        name: "FK_SeatInventories_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_ArrivalAirportCode",
                table: "Flights",
                column: "ArrivalAirportCode");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DepartureAirportCode",
                table: "Flights",
                column: "DepartureAirportCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeatInventories");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}

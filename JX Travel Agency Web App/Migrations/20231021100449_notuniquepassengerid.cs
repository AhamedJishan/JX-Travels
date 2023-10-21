using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JX_Travel_Agency_Web_App.Migrations
{
    public partial class notuniquepassengerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets",
                column: "PassengerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets",
                column: "PassengerId",
                unique: true);
        }
    }
}

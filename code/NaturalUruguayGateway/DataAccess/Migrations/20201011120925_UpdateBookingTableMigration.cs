using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class UpdateBookingTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Booking",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking",
                column: "ConfirmationCode",
                unique: true,
                filter: "[ConfirmationCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Booking");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class UpdateBookingPropertyTypoFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LodgmnetId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "LodgmentId",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking",
                column: "LodgmentId",
                principalTable: "Lodgment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking");

            migrationBuilder.AlterColumn<int>(
                name: "LodgmentId",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "LodgmnetId",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking",
                column: "LodgmentId",
                principalTable: "Lodgment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

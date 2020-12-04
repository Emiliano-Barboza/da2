using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class UpdateCompleteBookingTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "StatusDescription",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationCode",
                table: "Booking",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AmountGuest",
                table: "Booking",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<long>(
                name: "CheckIn",
                table: "Booking",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CheckOut",
                table: "Booking",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Booking",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Booking",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LodgmentId",
                table: "Booking",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LodgmnetId",
                table: "Booking",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Booking",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking",
                column: "ConfirmationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_LodgmentId",
                table: "Booking",
                column: "LodgmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking",
                column: "LodgmentId",
                principalTable: "Lodgment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Lodgment_LodgmentId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_LodgmentId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AmountGuest",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LodgmentId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "LodgmnetId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "StatusDescription",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationCode",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ConfirmationCode",
                table: "Booking",
                column: "ConfirmationCode",
                unique: true,
                filter: "[ConfirmationCode] IS NOT NULL");
        }
    }
}

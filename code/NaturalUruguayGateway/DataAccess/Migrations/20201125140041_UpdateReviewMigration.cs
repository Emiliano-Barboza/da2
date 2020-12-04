using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class UpdateReviewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfStarse",
                table: "Review");

            migrationBuilder.AddColumn<byte>(
                name: "AmountOfStars",
                table: "Review",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfStars",
                table: "Review");

            migrationBuilder.AddColumn<byte>(
                name: "AmountOfStarse",
                table: "Review",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class LodgmentIsActiveMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Lodgment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Lodgment");
        }
    }
}

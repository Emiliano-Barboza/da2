using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class SpotsInLodgmentsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lodgment_Name",
                table: "Lodgment");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Lodgment",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lodgment",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Lodgment",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lodgment",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactInformation",
                table: "Lodgment",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Lodgment",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpotId",
                table: "Lodgment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Spot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spot", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lodgment_Name",
                table: "Lodgment",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lodgment_SpotId",
                table: "Lodgment",
                column: "SpotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lodgment_Spot_SpotId",
                table: "Lodgment",
                column: "SpotId",
                principalTable: "Spot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lodgment_Spot_SpotId",
                table: "Lodgment");

            migrationBuilder.DropTable(
                name: "Spot");

            migrationBuilder.DropIndex(
                name: "IX_Lodgment_Name",
                table: "Lodgment");

            migrationBuilder.DropIndex(
                name: "IX_Lodgment_SpotId",
                table: "Lodgment");

            migrationBuilder.DropColumn(
                name: "SpotId",
                table: "Lodgment");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Lodgment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lodgment",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Images",
                table: "Lodgment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Lodgment",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "ContactInformation",
                table: "Lodgment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Lodgment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Lodgment_Name",
                table: "Lodgment",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}

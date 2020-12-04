using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class RegionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Spot",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Spot",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Spot",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Region",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Región metropolitana" },
                    { 2, "RRegión Centro Sur" },
                    { 3, "Región Este" },
                    { 4, "Región Litoral Norte" },
                    { 5, "Región “Corredor Pájaros Pintados”" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Spot_Name",
                table: "Spot",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spot_RegionId",
                table: "Spot",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_Name",
                table: "Region",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Spot_Region_RegionId",
                table: "Spot",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spot_Region_RegionId",
                table: "Spot");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Spot_Name",
                table: "Spot");

            migrationBuilder.DropIndex(
                name: "IX_Spot_RegionId",
                table: "Spot");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Spot");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Spot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Spot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);
        }
    }
}

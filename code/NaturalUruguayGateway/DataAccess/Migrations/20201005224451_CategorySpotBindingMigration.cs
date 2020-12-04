using Microsoft.EntityFrameworkCore.Migrations;

namespace NaturalUruguayGateway.DataAccess.Migrations
{
    public partial class CategorySpotBindingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategorySpot",
                table: "CategorySpot");

            migrationBuilder.DropIndex(
                name: "IX_CategorySpot_CategoryId",
                table: "CategorySpot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategorySpot",
                table: "CategorySpot",
                columns: new[] { "CategoryId", "SpotId" });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpot_SpotId",
                table: "CategorySpot",
                column: "SpotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategorySpot",
                table: "CategorySpot");

            migrationBuilder.DropIndex(
                name: "IX_CategorySpot_SpotId",
                table: "CategorySpot");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategorySpot",
                table: "CategorySpot",
                columns: new[] { "SpotId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpot_CategoryId",
                table: "CategorySpot",
                column: "CategoryId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTracking_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationTracking_BusinessId",
                table: "ApplicationTracking");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_BusinessId",
                table: "ApplicationTracking",
                column: "BusinessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationTracking_BusinessId",
                table: "ApplicationTracking");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationTracking_BusinessId",
                table: "ApplicationTracking",
                column: "BusinessId",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTracking_Realation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTracking_Businesses_BusinessId",
                table: "ApplicationTracking");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTrackingId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "ApplicationTracking",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Required",
                table: "ApplicationTracking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ApplicationTrackingId",
                table: "Notification",
                column: "ApplicationTrackingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTracking_Businesses_BusinessId",
                table: "ApplicationTracking",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_ApplicationTracking_ApplicationTrackingId",
                table: "Notification",
                column: "ApplicationTrackingId",
                principalTable: "ApplicationTracking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTracking_Businesses_BusinessId",
                table: "ApplicationTracking");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_ApplicationTracking_ApplicationTrackingId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_ApplicationTrackingId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "ApplicationTrackingId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "ApplicationTracking");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "ApplicationTracking",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTracking_Businesses_BusinessId",
                table: "ApplicationTracking",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class SupportAttachment_supportmessage_optional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment");

            migrationBuilder.AlterColumn<int>(
                name: "SupportMessageId",
                table: "SupportAttachment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment",
                column: "SupportMessageId",
                principalTable: "Support",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment");

            migrationBuilder.AlterColumn<int>(
                name: "SupportMessageId",
                table: "SupportAttachment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment",
                column: "SupportMessageId",
                principalTable: "Support",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class Support_Date_modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Support_AspNetUsers_SendById",
                table: "Support");

            migrationBuilder.DropForeignKey(
                name: "FK_Support_Support_ReplyToId",
                table: "Support");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportAttachment",
                table: "SupportAttachment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Support",
                table: "Support");

            migrationBuilder.RenameTable(
                name: "SupportAttachment",
                newName: "SupportAttachments");

            migrationBuilder.RenameTable(
                name: "Support",
                newName: "SupportMessages");

            migrationBuilder.RenameIndex(
                name: "IX_SupportAttachment_SupportMessageId",
                table: "SupportAttachments",
                newName: "IX_SupportAttachments_SupportMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Support_SendById",
                table: "SupportMessages",
                newName: "IX_SupportMessages_SendById");

            migrationBuilder.RenameIndex(
                name: "IX_Support_ReplyToId",
                table: "SupportMessages",
                newName: "IX_SupportMessages_ReplyToId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportAttachments",
                table: "SupportAttachments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportMessages",
                table: "SupportMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportAttachments_SupportMessages_SupportMessageId",
                table: "SupportAttachments",
                column: "SupportMessageId",
                principalTable: "SupportMessages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportMessages_AspNetUsers_SendById",
                table: "SupportMessages",
                column: "SendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportMessages_SupportMessages_ReplyToId",
                table: "SupportMessages",
                column: "ReplyToId",
                principalTable: "SupportMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportAttachments_SupportMessages_SupportMessageId",
                table: "SupportAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportMessages_AspNetUsers_SendById",
                table: "SupportMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportMessages_SupportMessages_ReplyToId",
                table: "SupportMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportMessages",
                table: "SupportMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportAttachments",
                table: "SupportAttachments");

            migrationBuilder.RenameTable(
                name: "SupportMessages",
                newName: "Support");

            migrationBuilder.RenameTable(
                name: "SupportAttachments",
                newName: "SupportAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_SupportMessages_SendById",
                table: "Support",
                newName: "IX_Support_SendById");

            migrationBuilder.RenameIndex(
                name: "IX_SupportMessages_ReplyToId",
                table: "Support",
                newName: "IX_Support_ReplyToId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportAttachments_SupportMessageId",
                table: "SupportAttachment",
                newName: "IX_SupportAttachment_SupportMessageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Support",
                table: "Support",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportAttachment",
                table: "SupportAttachment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Support_AspNetUsers_SendById",
                table: "Support",
                column: "SendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Support_Support_ReplyToId",
                table: "Support",
                column: "ReplyToId",
                principalTable: "Support",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportAttachment_Support_SupportMessageId",
                table: "SupportAttachment",
                column: "SupportMessageId",
                principalTable: "Support",
                principalColumn: "Id");
        }
    }
}

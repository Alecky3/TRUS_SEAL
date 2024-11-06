using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTracking_Comments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTracking_Businesses_BusinessId",
                table: "ApplicationTracking");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_ApplicationTracking_ApplicationTrackingId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_BsAnswer_BsAnswerId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_BusinessAttachment_BusinessAttachmentId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Businesses_BusinessId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Notification_NotificationId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_QuestionCategories_QuestionCategoryId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Questions_QuestionId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Seals_SealId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationTracking",
                table: "ApplicationTracking");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "ApplicationTracking",
                newName: "ApplicationTrackings");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SealId",
                table: "Notifications",
                newName: "IX_Notifications_SealId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_QuestionId",
                table: "Notifications",
                newName: "IX_Notifications_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_QuestionCategoryId",
                table: "Notifications",
                newName: "IX_Notifications_QuestionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationId",
                table: "Notifications",
                newName: "IX_Notifications_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_BusinessId",
                table: "Notifications",
                newName: "IX_Notifications_BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_BusinessAttachmentId",
                table: "Notifications",
                newName: "IX_Notifications_BusinessAttachmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_BsAnswerId",
                table: "Notifications",
                newName: "IX_Notifications_BsAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_ApplicationTrackingId",
                table: "Notifications",
                newName: "IX_Notifications_ApplicationTrackingId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationTracking_BusinessId",
                table: "ApplicationTrackings",
                newName: "IX_ApplicationTrackings_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationTrackings",
                table: "ApplicationTrackings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationTrackings_Businesses_BusinessId",
                table: "ApplicationTrackings",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ApplicationTrackings_ApplicationTrackingId",
                table: "Notifications",
                column: "ApplicationTrackingId",
                principalTable: "ApplicationTrackings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_BsAnswer_BsAnswerId",
                table: "Notifications",
                column: "BsAnswerId",
                principalTable: "BsAnswer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_BusinessAttachment_BusinessAttachmentId",
                table: "Notifications",
                column: "BusinessAttachmentId",
                principalTable: "BusinessAttachment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Businesses_BusinessId",
                table: "Notifications",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Notifications_NotificationId",
                table: "Notifications",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_QuestionCategories_QuestionCategoryId",
                table: "Notifications",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Questions_QuestionId",
                table: "Notifications",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Seals_SealId",
                table: "Notifications",
                column: "SealId",
                principalTable: "Seals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationTrackings_Businesses_BusinessId",
                table: "ApplicationTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ApplicationTrackings_ApplicationTrackingId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_BsAnswer_BsAnswerId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_BusinessAttachment_BusinessAttachmentId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Businesses_BusinessId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Notifications_NotificationId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_QuestionCategories_QuestionCategoryId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Questions_QuestionId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Seals_SealId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationTrackings",
                table: "ApplicationTrackings");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "ApplicationTrackings",
                newName: "ApplicationTracking");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_SealId",
                table: "Notification",
                newName: "IX_Notification_SealId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_QuestionId",
                table: "Notification",
                newName: "IX_Notification_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_QuestionCategoryId",
                table: "Notification",
                newName: "IX_Notification_QuestionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotificationId",
                table: "Notification",
                newName: "IX_Notification_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_BusinessId",
                table: "Notification",
                newName: "IX_Notification_BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_BusinessAttachmentId",
                table: "Notification",
                newName: "IX_Notification_BusinessAttachmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_BsAnswerId",
                table: "Notification",
                newName: "IX_Notification_BsAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ApplicationTrackingId",
                table: "Notification",
                newName: "IX_Notification_ApplicationTrackingId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationTrackings_BusinessId",
                table: "ApplicationTracking",
                newName: "IX_ApplicationTracking_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationTracking",
                table: "ApplicationTracking",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_BsAnswer_BsAnswerId",
                table: "Notification",
                column: "BsAnswerId",
                principalTable: "BsAnswer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_BusinessAttachment_BusinessAttachmentId",
                table: "Notification",
                column: "BusinessAttachmentId",
                principalTable: "BusinessAttachment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Businesses_BusinessId",
                table: "Notification",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Notification_NotificationId",
                table: "Notification",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_QuestionCategories_QuestionCategoryId",
                table: "Notification",
                column: "QuestionCategoryId",
                principalTable: "QuestionCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Questions_QuestionId",
                table: "Notification",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Seals_SealId",
                table: "Notification",
                column: "SealId",
                principalTable: "Seals",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class NotificationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BusinessId = table.Column<int>(type: "int", nullable: true),
                    BsAnswerId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    QuestionCategoryId = table.Column<int>(type: "int", nullable: true),
                    SealId = table.Column<int>(type: "int", nullable: true),
                    BusinessAttachmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_BsAnswer_BsAnswerId",
                        column: x => x.BsAnswerId,
                        principalTable: "BsAnswer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_BusinessAttachment_BusinessAttachmentId",
                        column: x => x.BusinessAttachmentId,
                        principalTable: "BusinessAttachment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_QuestionCategories_QuestionCategoryId",
                        column: x => x.QuestionCategoryId,
                        principalTable: "QuestionCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Seals_SealId",
                        column: x => x.SealId,
                        principalTable: "Seals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_BsAnswerId",
                table: "Notification",
                column: "BsAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_BusinessAttachmentId",
                table: "Notification",
                column: "BusinessAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_BusinessId",
                table: "Notification",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationId",
                table: "Notification",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_QuestionCategoryId",
                table: "Notification",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_QuestionId",
                table: "Notification",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SealId",
                table: "Notification",
                column: "SealId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}

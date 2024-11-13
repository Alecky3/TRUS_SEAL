using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class DisplayNameAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "BusinessAttachment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "BusinessAttachment");
        }
    }
}

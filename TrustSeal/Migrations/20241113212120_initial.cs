using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrustSeal.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForWhichField",
                table: "BusinessAttachment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForWhichField",
                table: "BusinessAttachment");
        }
    }
}

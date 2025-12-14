using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YumaIdentity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAllowedCallbackUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedCallbackUrls",
                table: "Applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedCallbackUrls",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

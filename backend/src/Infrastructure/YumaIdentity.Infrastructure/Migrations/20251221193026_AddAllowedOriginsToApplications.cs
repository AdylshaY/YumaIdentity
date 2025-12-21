using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YumaIdentity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedOriginsToApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedOrigins",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedOrigins",
                table: "Applications");
        }
    }
}

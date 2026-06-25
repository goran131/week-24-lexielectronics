using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiElectronics.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccesstoApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Access",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "AspNetUsers");
        }
    }
}

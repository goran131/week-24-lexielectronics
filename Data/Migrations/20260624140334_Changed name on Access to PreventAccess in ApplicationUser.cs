using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiElectronics.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangednameonAccesstoPreventAccessinApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Access",
                table: "AspNetUsers",
                newName: "PreventAccess");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreventAccess",
                table: "AspNetUsers",
                newName: "Access");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiElectronics.Data.Migrations
{
    /// <inheritdoc />
    public partial class ÄndratZipCodetillZipcodeiApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceZipCode",
                table: "AspNetUsers",
                newName: "InvoiceZipcode");

            migrationBuilder.RenameColumn(
                name: "DeliveryZipCode",
                table: "AspNetUsers",
                newName: "DeliveryZipcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceZipcode",
                table: "AspNetUsers",
                newName: "InvoiceZipCode");

            migrationBuilder.RenameColumn(
                name: "DeliveryZipcode",
                table: "AspNetUsers",
                newName: "DeliveryZipCode");
        }
    }
}

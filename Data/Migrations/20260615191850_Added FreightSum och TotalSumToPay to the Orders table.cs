using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LexiElectronics.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFreightSumochTotalSumToPaytotheOrderstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreightSum",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreightSum",
                table: "Orders");
        }
    }
}

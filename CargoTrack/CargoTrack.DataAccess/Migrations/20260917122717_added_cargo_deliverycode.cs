using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoTrack.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class added_cargo_deliverycode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryCode",
                table: "Cargos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCode",
                table: "Cargos");
        }
    }
}

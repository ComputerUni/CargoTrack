using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoTrack.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class add_deliveryaddress_and_assignedemployee_to_cargo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedEmployeeId",
                table: "Cargos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "Cargos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_AssignedEmployeeId",
                table: "Cargos",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_DeliveryAddressId",
                table: "Cargos",
                column: "DeliveryAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_Addresses_DeliveryAddressId",
                table: "Cargos",
                column: "DeliveryAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_Employees_AssignedEmployeeId",
                table: "Cargos",
                column: "AssignedEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_Addresses_DeliveryAddressId",
                table: "Cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_Employees_AssignedEmployeeId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_AssignedEmployeeId",
                table: "Cargos");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_DeliveryAddressId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "AssignedEmployeeId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "Cargos");
        }
    }
}

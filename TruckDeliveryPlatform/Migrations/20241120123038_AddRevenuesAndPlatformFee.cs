using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddRevenuesAndPlatformFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PlatformFee",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Revenue",
                table: "SystemWallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 20, 12, 30, 37, 553, DateTimeKind.Utc).AddTicks(6711));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlatformFee",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "SystemWallets");

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 19, 21, 4, 52, 959, DateTimeKind.Utc).AddTicks(7853));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class SyetemFeesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TransactionFee",
                table: "SystemConfigs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "TransactionFee" },
                values: new object[] { new DateTime(2024, 11, 21, 11, 48, 6, 756, DateTimeKind.Utc).AddTicks(2985), 10m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionFee",
                table: "SystemConfigs");

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 20, 12, 30, 37, 553, DateTimeKind.Utc).AddTicks(6711));
        }
    }
}

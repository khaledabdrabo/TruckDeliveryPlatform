using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToTruckOwnerProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TruckOwnerProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TruckOwnerProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SystemConfigs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SystemConfigs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 19, 21, 4, 52, 959, DateTimeKind.Utc).AddTicks(7853), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TruckOwnerProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TruckOwnerProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SystemConfigs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SystemConfigs");
        }
    }
}

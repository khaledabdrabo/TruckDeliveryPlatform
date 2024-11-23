using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddWaitingHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WaitingHourPrice",
                table: "TruckOwnerProfiles",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedWaitingHours",
                table: "Jobs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WaitingHourPrice",
                table: "Bids",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitingHourPrice",
                table: "TruckOwnerProfiles");

            migrationBuilder.DropColumn(
                name: "EstimatedWaitingHours",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WaitingHourPrice",
                table: "Bids");
        }
    }
}

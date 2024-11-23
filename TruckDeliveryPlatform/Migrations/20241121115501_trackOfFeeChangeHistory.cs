using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class trackOfFeeChangeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionFeeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionFeeHistory", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 21, 11, 55, 0, 345, DateTimeKind.Utc).AddTicks(3539));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionFeeHistory");

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 21, 11, 48, 6, 756, DateTimeKind.Utc).AddTicks(2985));
        }
    }
}

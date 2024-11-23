using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddTripFeedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    TruckOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerRating = table.Column<int>(type: "int", nullable: false),
                    CustomerCooperation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripFeedbacks_AspNetUsers_TruckOwnerId",
                        column: x => x.TruckOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripFeedbacks_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 23, 21, 29, 12, 855, DateTimeKind.Utc).AddTicks(4173));

            migrationBuilder.CreateIndex(
                name: "IX_TripFeedbacks_JobId",
                table: "TripFeedbacks",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_TripFeedbacks_TruckOwnerId",
                table: "TripFeedbacks",
                column: "TruckOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripFeedbacks");

            migrationBuilder.UpdateData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 11, 21, 12, 43, 39, 62, DateTimeKind.Utc).AddTicks(4511));
        }
    }
}

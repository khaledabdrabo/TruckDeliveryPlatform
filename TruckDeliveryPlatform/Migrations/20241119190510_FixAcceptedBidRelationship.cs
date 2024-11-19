using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixAcceptedBidRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Bids_AcceptedBidId1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_AcceptedBidId1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "AcceptedBidId1",
                table: "Jobs");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AcceptedBidId",
                table: "Jobs",
                column: "AcceptedBidId",
                unique: true,
                filter: "[AcceptedBidId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Bids_AcceptedBidId",
                table: "Jobs",
                column: "AcceptedBidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Bids_AcceptedBidId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_AcceptedBidId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "AcceptedBidId1",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_AcceptedBidId1",
                table: "Jobs",
                column: "AcceptedBidId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Bids_AcceptedBidId1",
                table: "Jobs",
                column: "AcceptedBidId1",
                principalTable: "Bids",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_TruckOwnerProfiles_TruckOwnerProfileId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_TruckOwnerProfileId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "TruckOwnerProfileId",
                table: "Locations");

            migrationBuilder.CreateTable(
                name: "LocationTruckOwnerProfile",
                columns: table => new
                {
                    ServiceAreasId = table.Column<int>(type: "int", nullable: false),
                    TruckOwnerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTruckOwnerProfile", x => new { x.ServiceAreasId, x.TruckOwnerProfileId });
                    table.ForeignKey(
                        name: "FK_LocationTruckOwnerProfile_Locations_ServiceAreasId",
                        column: x => x.ServiceAreasId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationTruckOwnerProfile_TruckOwnerProfiles_TruckOwnerProfileId",
                        column: x => x.TruckOwnerProfileId,
                        principalTable: "TruckOwnerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationTruckOwnerProfile_TruckOwnerProfileId",
                table: "LocationTruckOwnerProfile",
                column: "TruckOwnerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationTruckOwnerProfile");

            migrationBuilder.AddColumn<int>(
                name: "TruckOwnerProfileId",
                table: "Locations",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 10,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 11,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 12,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 13,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 14,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 15,
                column: "TruckOwnerProfileId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TruckOwnerProfileId",
                table: "Locations",
                column: "TruckOwnerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_TruckOwnerProfiles_TruckOwnerProfileId",
                table: "Locations",
                column: "TruckOwnerProfileId",
                principalTable: "TruckOwnerProfiles",
                principalColumn: "Id");
        }
    }
}

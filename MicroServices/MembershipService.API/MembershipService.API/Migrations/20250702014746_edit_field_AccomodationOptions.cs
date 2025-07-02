using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class edit_field_AccomodationOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "AccommodationOptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Rooms",
                newName: "DescriptionDetails");

            migrationBuilder.RenameColumn(
                name: "QuantityAvailable",
                table: "AccommodationOptions",
                newName: "RoomTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "Floor",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationOptionId",
                table: "MediaGallery",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "AccommodationOptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_LocationId",
                table: "AccommodationOptions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_RoomTypeId",
                table: "AccommodationOptions",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationOptions_Locations_LocationId",
                table: "AccommodationOptions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationOptions_RoomType_RoomTypeId",
                table: "AccommodationOptions",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId",
                principalTable: "AccommodationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationOptions_Locations_LocationId",
                table: "AccommodationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationOptions_RoomType_RoomTypeId",
                table: "AccommodationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropTable(
                name: "RoomType");

            migrationBuilder.DropIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationOptions_LocationId",
                table: "AccommodationOptions");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationOptions_RoomTypeId",
                table: "AccommodationOptions");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "AccommodationOptions");

            migrationBuilder.RenameColumn(
                name: "DescriptionDetails",
                table: "Rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "AccommodationOptions",
                newName: "QuantityAvailable");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "AccommodationOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

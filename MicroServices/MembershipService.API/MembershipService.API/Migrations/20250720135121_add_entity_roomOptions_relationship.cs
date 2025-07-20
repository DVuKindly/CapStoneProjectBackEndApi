using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_entity_roomOptions_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Rooms");

            migrationBuilder.AddColumn<float>(
                name: "AreaInSquareMeters",
                table: "Rooms",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BedTypeOptionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBeds",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomFloorOptionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomSizeOptionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomViewOptionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BedTypeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedTypeOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomFloorOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFloorOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomSizeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSizeOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomViewOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomViewOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BedTypeOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Single" },
                    { 2, "Double" },
                    { 3, "Queen" },
                    { 4, "King" }
                });

            migrationBuilder.InsertData(
                table: "RoomFloorOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Low" },
                    { 2, "Mid" },
                    { 3, "High" },
                    { 4, "Terrace" }
                });

            migrationBuilder.InsertData(
                table: "RoomSizeOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Small" },
                    { 2, "Medium" },
                    { 3, "Large" }
                });

            migrationBuilder.InsertData(
                table: "RoomViewOptions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Garden" },
                    { 2, "Sea" },
                    { 3, "Balcony" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BedTypeOptionId",
                table: "Rooms",
                column: "BedTypeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomFloorOptionId",
                table: "Rooms",
                column: "RoomFloorOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomSizeOptionId",
                table: "Rooms",
                column: "RoomSizeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomViewOptionId",
                table: "Rooms",
                column: "RoomViewOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_BedTypeOptions_BedTypeOptionId",
                table: "Rooms",
                column: "BedTypeOptionId",
                principalTable: "BedTypeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomFloorOptions_RoomFloorOptionId",
                table: "Rooms",
                column: "RoomFloorOptionId",
                principalTable: "RoomFloorOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomSizeOptions_RoomSizeOptionId",
                table: "Rooms",
                column: "RoomSizeOptionId",
                principalTable: "RoomSizeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomViewOptions_RoomViewOptionId",
                table: "Rooms",
                column: "RoomViewOptionId",
                principalTable: "RoomViewOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_BedTypeOptions_BedTypeOptionId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomFloorOptions_RoomFloorOptionId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomSizeOptions_RoomSizeOptionId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomViewOptions_RoomViewOptionId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "BedTypeOptions");

            migrationBuilder.DropTable(
                name: "RoomFloorOptions");

            migrationBuilder.DropTable(
                name: "RoomSizeOptions");

            migrationBuilder.DropTable(
                name: "RoomViewOptions");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BedTypeOptionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomFloorOptionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomSizeOptionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomViewOptionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AreaInSquareMeters",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BedTypeOptionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumberOfBeds",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomFloorOptionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomSizeOptionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomViewOptionId",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

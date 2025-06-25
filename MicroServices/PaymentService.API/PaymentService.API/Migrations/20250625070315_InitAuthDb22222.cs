using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitAuthDb22222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDirectMembership",
                table: "PaymentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirectMembership",
                table: "PaymentRequests");
        }
    }
}

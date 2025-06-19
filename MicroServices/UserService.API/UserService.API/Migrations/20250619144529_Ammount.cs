using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class Ammount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "PendingMembershipRequests",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PendingMembershipRequests");
        }
    }
}

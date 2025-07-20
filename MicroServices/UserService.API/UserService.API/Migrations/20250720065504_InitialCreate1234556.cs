using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1234556 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExtraFee",
                table: "PendingMembershipRequests",
                newName: "AddOnsFee");

            migrationBuilder.AddColumn<decimal>(
                name: "AddOnsFee",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddOnsFee",
                table: "Memberships");

            migrationBuilder.RenameColumn(
                name: "AddOnsFee",
                table: "PendingMembershipRequests",
                newName: "ExtraFee");
        }
    }
}

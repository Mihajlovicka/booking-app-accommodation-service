using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationService.Migrations
{
    /// <inheritdoc />
    public partial class AutomaticReservationApproveOnAccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutomaticReservation",
                table: "Accommodations",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutomaticReservation",
                table: "Accommodations");
        }
    }
}

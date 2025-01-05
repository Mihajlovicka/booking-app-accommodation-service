using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationService.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_addresses_street_number_street_name_city_country",
                table: "addresses");

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    accommodation_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.id);
                    table.ForeignKey(
                        name: "FK_Picture_Accommodations_accommodation_id",
                        column: x => x.accommodation_id,
                        principalTable: "Accommodations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_street_number_street_name_city_country",
                table: "addresses",
                columns: new[] { "street_number", "street_name", "city", "country" });

            migrationBuilder.CreateIndex(
                name: "IX_Picture_accommodation_id",
                table: "Picture",
                column: "accommodation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_addresses_street_number_street_name_city_country",
                table: "addresses");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_street_number_street_name_city_country",
                table: "addresses",
                columns: new[] { "street_number", "street_name", "city", "country" },
                unique: true);
        }
    }
}

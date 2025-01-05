using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccommodationService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEquipmentModel4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accommodations_AspNetUsers_owner_id",
                table: "accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_accommodations_addresses_address_id",
                table: "accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_accommodations_AccommodationId",
                table: "equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_accommodations_AccommodationId1",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_AccommodationId",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_AccommodationId1",
                table: "equipment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accommodations",
                table: "accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "AccommodationId1",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "AccommodationId2",
                table: "equipment");

            migrationBuilder.RenameTable(
                name: "accommodations",
                newName: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_accommodations_owner_id",
                table: "Accommodations",
                newName: "IX_Accommodations_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_accommodations_address_id",
                table: "Accommodations",
                newName: "IX_Accommodations_address_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "external_id",
                table: "Accommodations",
                type: "char(16)",
                maxLength: 16,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(16)",
                oldMaxLength: 16)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "id");

            migrationBuilder.CreateTable(
                name: "AccommodationEquipments",
                columns: table => new
                {
                    AccommodationsId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationEquipments", x => new { x.AccommodationsId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_AccommodationEquipments_Accommodations_AccommodationsId",
                        column: x => x.AccommodationsId,
                        principalTable: "Accommodations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationEquipments_equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationEquipments_EquipmentId",
                table: "AccommodationEquipments",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AspNetUsers_owner_id",
                table: "Accommodations",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_addresses_address_id",
                table: "Accommodations",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AspNetUsers_owner_id",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_addresses_address_id",
                table: "Accommodations");

            migrationBuilder.DropTable(
                name: "AccommodationEquipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.RenameTable(
                name: "Accommodations",
                newName: "accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_owner_id",
                table: "accommodations",
                newName: "IX_accommodations_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_address_id",
                table: "accommodations",
                newName: "IX_accommodations_address_id");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationId",
                table: "equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccommodationId1",
                table: "equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccommodationId2",
                table: "equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "external_id",
                table: "accommodations",
                type: "char(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accommodations",
                table: "accommodations",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_AccommodationId",
                table: "equipment",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_AccommodationId1",
                table: "equipment",
                column: "AccommodationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_accommodations_AspNetUsers_owner_id",
                table: "accommodations",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_accommodations_addresses_address_id",
                table: "accommodations",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_accommodations_AccommodationId",
                table: "equipment",
                column: "AccommodationId",
                principalTable: "accommodations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_accommodations_AccommodationId1",
                table: "equipment",
                column: "AccommodationId1",
                principalTable: "accommodations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdresIdeklendisiparise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "adresId",
                table: "Siparisler",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Siparisler_adresId",
                table: "Siparisler",
                column: "adresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Adresler_adresId",
                table: "Siparisler",
                column: "adresId",
                principalTable: "Adresler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Adresler_adresId",
                table: "Siparisler");

            migrationBuilder.DropIndex(
                name: "IX_Siparisler_adresId",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "adresId",
                table: "Siparisler");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdresIdeklendisiparise2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Adresler_adresId",
                table: "Siparisler");

            migrationBuilder.RenameColumn(
                name: "adresId",
                table: "Siparisler",
                newName: "AdresId");

            migrationBuilder.RenameIndex(
                name: "IX_Siparisler_adresId",
                table: "Siparisler",
                newName: "IX_Siparisler_AdresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Adresler_AdresId",
                table: "Siparisler",
                column: "AdresId",
                principalTable: "Adresler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Adresler_AdresId",
                table: "Siparisler");

            migrationBuilder.RenameColumn(
                name: "AdresId",
                table: "Siparisler",
                newName: "adresId");

            migrationBuilder.RenameIndex(
                name: "IX_Siparisler_AdresId",
                table: "Siparisler",
                newName: "IX_Siparisler_adresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Adresler_adresId",
                table: "Siparisler",
                column: "adresId",
                principalTable: "Adresler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

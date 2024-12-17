using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiparisDetaylar_Urunler_UrunId",
                table: "SiparisDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Restoranlar_RestoranId",
                table: "Siparisler");

            migrationBuilder.AddColumn<bool>(
                name: "yorumYapildiMi",
                table: "Siparisler",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SiparisDetaylar_Urunler_UrunId",
                table: "SiparisDetaylar",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Restoranlar_RestoranId",
                table: "Siparisler",
                column: "RestoranId",
                principalTable: "Restoranlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiparisDetaylar_Urunler_UrunId",
                table: "SiparisDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_Siparisler_Restoranlar_RestoranId",
                table: "Siparisler");

            migrationBuilder.DropColumn(
                name: "yorumYapildiMi",
                table: "Siparisler");

            migrationBuilder.AddForeignKey(
                name: "FK_SiparisDetaylar_Urunler_UrunId",
                table: "SiparisDetaylar",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Siparisler_Restoranlar_RestoranId",
                table: "Siparisler",
                column: "RestoranId",
                principalTable: "Restoranlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restoranlar_KategoriRestoranlar_KategoriRestoranId",
                table: "Restoranlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KategoriRestoranlar",
                table: "KategoriRestoranlar");

            migrationBuilder.RenameTable(
                name: "KategoriRestoranlar",
                newName: "RestoranKategori");

            migrationBuilder.RenameColumn(
                name: "KategoriRestoranId",
                table: "RestoranKategori",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "RestoranKategori",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestoranKategori",
                table: "RestoranKategori",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restoranlar_RestoranKategori_KategoriRestoranId",
                table: "Restoranlar",
                column: "KategoriRestoranId",
                principalTable: "RestoranKategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restoranlar_RestoranKategori_KategoriRestoranId",
                table: "Restoranlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestoranKategori",
                table: "RestoranKategori");

            migrationBuilder.RenameTable(
                name: "RestoranKategori",
                newName: "KategoriRestoranlar");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "KategoriRestoranlar",
                newName: "KategoriRestoranId");

            migrationBuilder.AlterColumn<string>(
                name: "Ad",
                table: "KategoriRestoranlar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KategoriRestoranlar",
                table: "KategoriRestoranlar",
                column: "KategoriRestoranId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restoranlar_KategoriRestoranlar_KategoriRestoranId",
                table: "Restoranlar",
                column: "KategoriRestoranId",
                principalTable: "KategoriRestoranlar",
                principalColumn: "KategoriRestoranId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

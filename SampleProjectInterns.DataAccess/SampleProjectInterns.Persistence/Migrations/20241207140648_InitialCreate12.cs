using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KategoriRestoranlar",
                columns: table => new
                {
                    KategoriRestoranId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategoriRestoranlar", x => x.KategoriRestoranId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restoranlar_KategoriRestoranId",
                table: "Restoranlar",
                column: "KategoriRestoranId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restoranlar_KategoriRestoranlar_KategoriRestoranId",
                table: "Restoranlar",
                column: "KategoriRestoranId",
                principalTable: "KategoriRestoranlar",
                principalColumn: "KategoriRestoranId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restoranlar_KategoriRestoranlar_KategoriRestoranId",
                table: "Restoranlar");

            migrationBuilder.DropTable(
                name: "KategoriRestoranlar");

            migrationBuilder.DropIndex(
                name: "IX_Restoranlar_KategoriRestoranId",
                table: "Restoranlar");
        }
    }
}

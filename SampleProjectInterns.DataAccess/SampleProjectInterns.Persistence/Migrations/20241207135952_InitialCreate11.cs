using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OdemeId",
                table: "Siparisler");

            migrationBuilder.AddColumn<int>(
                name: "KategoriRestoranId",
                table: "Restoranlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KategoriRestoranId",
                table: "Restoranlar");

            migrationBuilder.AddColumn<long>(
                name: "OdemeId",
                table: "Siparisler",
                type: "bigint",
                nullable: true);
        }
    }
}

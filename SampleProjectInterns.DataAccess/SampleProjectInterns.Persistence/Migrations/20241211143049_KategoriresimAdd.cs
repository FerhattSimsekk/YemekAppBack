using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleProjectInterns.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class KategoriresimAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResimUrl",
                table: "RestoranKategori",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResimUrl",
                table: "RestoranKategori");
        }
    }
}

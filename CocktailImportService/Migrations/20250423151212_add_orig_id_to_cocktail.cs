using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailImportService.Migrations
{
    /// <inheritdoc />
    public partial class add_orig_id_to_cocktail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Cocktails",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "OrigId",
                table: "Cocktails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrigId",
                table: "Cocktails");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Cocktails",
                newName: "Description");
        }
    }
}

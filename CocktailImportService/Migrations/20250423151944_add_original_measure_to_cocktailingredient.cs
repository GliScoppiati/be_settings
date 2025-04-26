using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailImportService.Migrations
{
    /// <inheritdoc />
    public partial class add_original_measure_to_cocktailingredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalMeasure",
                table: "CocktailIngredients",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalMeasure",
                table: "CocktailIngredients");
        }
    }
}

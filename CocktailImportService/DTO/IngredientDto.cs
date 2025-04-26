using System.Text.Json.Serialization;

namespace CocktailImportService.DTO
{
    public class IngredientDto
    {
        [JsonPropertyName("strIngredient1")]
        public string? Name { get; set; }
    }
}
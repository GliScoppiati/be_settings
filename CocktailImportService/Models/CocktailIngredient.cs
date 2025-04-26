using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocktailImportService.Models
{
    public class CocktailIngredient
    {
        public Guid Id { get; set; }

        // FK verso Cocktail
        [Required] public Guid CocktailId { get; set; }
        [ForeignKey(nameof(CocktailId))] public Cocktail? Cocktail { get; set; }

        // FK verso Ingredient
        [Required] public Guid IngredientId { get; set; }
        [ForeignKey(nameof(IngredientId))] public Ingredient? Ingredient { get; set; }

        // parsing “1 oz”, “½ shot” ecc.
        public string? QuantityUnit  { get; set; }
        public decimal? QuantityValue { get; set; }

        // misura originale così com’è nella API
        public string? OriginalMeasure { get; set; }
    }
}

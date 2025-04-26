using System.ComponentModel.DataAnnotations;

namespace CocktailImportService.Models
{
    public class Ingredient
    {
        [Key]
        public Guid IngredientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<CocktailIngredient> CocktailIngredients { get; set; } = new List<CocktailIngredient>();
    }
}
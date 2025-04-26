using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocktailImportService.Models
{
    public class Cocktail
    {
        [Key]
        public Guid CocktailId { get; set; }
        public string OrigId { get; set; } = string.Empty;
        public string Name        { get; set; } = string.Empty;
        public string? Category   { get; set; }
        public bool   IsAlcoholic { get; set; }
        public string? Glass      { get; set; }
        public string? Instructions { get; set; }
        public string? ImageUrl     { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<CocktailIngredient> CocktailIngredients { get; set; } = new List<CocktailIngredient>();
    }
}
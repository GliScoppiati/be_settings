using Microsoft.EntityFrameworkCore;

namespace CocktailImportService.Models
{
    public class CocktailDbContext : DbContext
    {
        public CocktailDbContext(DbContextOptions<CocktailDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cocktail> Cocktails { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<CocktailIngredient> CocktailIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CocktailIngredient>()
                .HasKey(ci => new { ci.CocktailId, ci.IngredientId });

            modelBuilder.Entity<CocktailIngredient>()
                .HasOne(ci => ci.Cocktail)
                .WithMany(c => c.CocktailIngredients)
                .HasForeignKey(ci => ci.CocktailId);

            modelBuilder.Entity<CocktailIngredient>()
                .HasOne(ci => ci.Ingredient)
                .WithMany(i => i.CocktailIngredients)
                .HasForeignKey(ci => ci.IngredientId);
        }
    }
}

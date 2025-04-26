using CocktailImportService.DTO;
using CocktailImportService.Models;
using Microsoft.EntityFrameworkCore;

namespace CocktailImportService.Services;

public class CocktailImporter
{
    private readonly CocktailApiService        _api;
    private readonly CocktailDbContext         _db;
    private readonly ILogger<CocktailImporter> _logger;

    public CocktailImporter(
        CocktailApiService        api,
        CocktailDbContext         db,
        ILogger<CocktailImporter> logger)
    {
        _api    = api;
        _db     = db;
        _logger = logger;
    }

    private static readonly char[] Letters = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

    public async Task<int> RunAsync(CancellationToken ct = default)
    {
        int totalInserted = 0;

        foreach (var ch in Letters)
        {
            var list = await _api.GetCocktailsByFirstLetterAsync(ch);
            if (list.Count == 0) continue;

            int insertedThisLetter = 0;

            foreach (var dto in list.Where(d => !string.IsNullOrWhiteSpace(d.IdDrink)))
            {
                if (await _db.Cocktails.AnyAsync(c => c.OrigId == dto.IdDrink, ct))
                    continue;

                var cocktail = new Cocktail
                {
                    CocktailId   = Guid.NewGuid(),
                    OrigId       = dto.IdDrink!,
                    Name         = dto.StrDrink ?? "(no-name)",
                    Category     = dto.StrCategory,
                    IsAlcoholic  = string.Equals(dto.StrAlcoholic, "Alcoholic",
                                                 StringComparison.OrdinalIgnoreCase),
                    Glass        = dto.StrGlass,
                    Instructions = dto.StrInstructions,
                    ImageUrl     = dto.StrDrinkThumb
                };

                // ---------- ingredienti ----------
                var ingNames = new[]
                {
                    dto.StrIngredient1, dto.StrIngredient2, dto.StrIngredient3, dto.StrIngredient4, dto.StrIngredient5,
                    dto.StrIngredient6, dto.StrIngredient7, dto.StrIngredient8, dto.StrIngredient9, dto.StrIngredient10,
                    dto.StrIngredient11,dto.StrIngredient12,dto.StrIngredient13,dto.StrIngredient14,dto.StrIngredient15
                };

                var measures = new[]
                {
                    dto.StrMeasure1, dto.StrMeasure2, dto.StrMeasure3, dto.StrMeasure4, dto.StrMeasure5,
                    dto.StrMeasure6, dto.StrMeasure7, dto.StrMeasure8, dto.StrMeasure9, dto.StrMeasure10,
                    dto.StrMeasure11,dto.StrMeasure12,dto.StrMeasure13,dto.StrMeasure14,dto.StrMeasure15
                };

                /* --- nuovo set per evitare duplicati locali ------------------ */
                var addedIngredientIds = new HashSet<Guid>();
                /* ------------------------------------------------------------- */

                for (int i = 0; i < ingNames.Length; i++)
                {
                    var ingName = ingNames[i]?.Trim();
                    if (string.IsNullOrWhiteSpace(ingName)) continue;

                    var ingredient = await _db.Ingredients
                                              .FirstOrDefaultAsync(x => x.Name == ingName, ct);

                    if (ingredient is null)
                    {
                        ingredient = new Ingredient
                        {
                            IngredientId = Guid.NewGuid(),
                            Name         = ingName
                        };
                        _db.Ingredients.Add(ingredient);
                    }

                    /* ---- skip se già aggiunto al cocktail -------------------- */
                    if (!addedIngredientIds.Add(ingredient.IngredientId))
                        continue;
                    /* ---------------------------------------------------------- */

                    cocktail.CocktailIngredients.Add(new CocktailIngredient
                    {
                        CocktailId      = cocktail.CocktailId,
                        IngredientId    = ingredient.IngredientId,
                        OriginalMeasure = measures[i]
                    });
                }
                // ---------- fine ingredienti ----------

                _db.Cocktails.Add(cocktail);
                insertedThisLetter++;
            }

            if (insertedThisLetter > 0)
            {
                await _db.SaveChangesAsync(ct);
                _db.ChangeTracker.Clear();
            }

            totalInserted += insertedThisLetter;
            _logger.LogInformation("✔️  Lettera {Letter}: importati {Count} cocktail nuovi",
                                   ch, insertedThisLetter);
        }

        _logger.LogInformation("🏁 Import terminato: {Total} cocktail inseriti", totalInserted);
        return totalInserted;
    }
}

using CocktailImportService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CocktailImportService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly CocktailImporter _importer;

    public ImportController(CocktailImporter importer)
    {
        _importer = importer;
    }

    /// <summary>Esegue lo scraper e restituisce quanti cocktail sono stati inseriti.</summary>
    [HttpPost("run")]
    public async Task<IActionResult> Run(CancellationToken ct)
    {
        var inserted = await _importer.RunAsync(ct);
        return Ok(new { inserted });
    }
}

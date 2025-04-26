using Microsoft.AspNetCore.Mvc;
using CocktailImportService.Services;

namespace CocktailImportService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CocktailTestController : ControllerBase
    {
        private readonly CocktailApiService _apiService;

        public CocktailTestController(CocktailApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _apiService.GetAllCategoriesAsync();
            return Ok(data);
        }

        [HttpGet("glasses")]
        public async Task<IActionResult> GetGlasses()
        {
            var data = await _apiService.GetAllGlassesAsync();
            return Ok(data);
        }

        [HttpGet("ingredients")]
        public async Task<IActionResult> GetIngredients()
        {
            var data = await _apiService.GetAllIngredientsAsync();
            return Ok(data);
        }

        [HttpGet("by-letter/{letter}")]
        public async Task<IActionResult> GetCocktailsByLetter(string letter)
        {
            if (string.IsNullOrWhiteSpace(letter) || letter.Length != 1)
                return BadRequest("Passa una sola lettera (a-z).");

            var data = await _apiService.GetCocktailsByFirstLetterAsync(letter[0]);
            return Ok(data);
        }
    }
}

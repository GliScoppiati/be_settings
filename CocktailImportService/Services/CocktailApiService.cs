using System.Net.Http.Json;
using CocktailImportService.DTO;


namespace CocktailImportService.Services
{
    public class CocktailApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CocktailApiService> _logger;

        public CocktailApiService(IHttpClientFactory httpClientFactory, ILogger<CocktailApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("TheCocktailDb");
            _logger = logger;
        }

        public async Task<List<string>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CategoriesResponse>("list.php?c=list");
                return response?.Drinks.Select(d => d.StrCategory).Where(c => !string.IsNullOrWhiteSpace(c)).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero delle categorie");
                return new();
            }
        }

        public async Task<List<string>> GetAllGlassesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GlassesResponse>("list.php?g=list");
                return response?.Drinks.Select(d => d.StrGlass).Where(g => !string.IsNullOrWhiteSpace(g)).ToList() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero dei bicchieri");
                return new();
            }
        }

        public async Task<List<string>> GetAllIngredientsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IngredientsResponse>("list.php?i=list");
                return response?.Drinks.Select(d => d.Name).Where(i => !string.IsNullOrWhiteSpace(i)).Cast<string>().ToList() ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero degli ingredienti");
                return new();
            }
        }

        public async Task<List<CocktailDto>> GetCocktailsByFirstLetterAsync(char letter)
        {
            try
            {
                var url = $"search.php?f={char.ToLowerInvariant(letter)}";
                var response = await _httpClient.GetFromJsonAsync<CocktailSearchResult>(url);

                // restituisco sempre una lista, mai null
                return response?.Drinks ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore nel recupero dei cocktail per lettera {letter}", letter);
                return new();
            }
        }
    }
}

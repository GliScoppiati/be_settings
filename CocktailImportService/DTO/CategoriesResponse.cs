namespace CocktailImportService.DTO
{
    public class CategoriesResponse
    {
        public List<CategoryDto> Drinks { get; set; } = new();
    }

    public class CategoryDto
    {
        public string StrCategory { get; set; } = string.Empty;
    }
}

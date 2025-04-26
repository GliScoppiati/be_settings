using CocktailImportService.Models;
using Microsoft.EntityFrameworkCore;
using CocktailImportService.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Connessione al DB
builder.Services.AddDbContext<CocktailDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ HTTP Client per TheCocktailDB
builder.Services.AddHttpClient("TheCocktailDb", client =>
{
    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");
});
builder.Services.AddScoped<CocktailApiService>();

// ✅ Controller
builder.Services.AddControllers();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CocktailImporter>();

var app = builder.Build();

// ✅ Migrations automatiche (con retry se DB non è ancora pronto)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CocktailDbContext>();
    int retries = 0;
    while (true)
    {
        try
        {
            db.Database.Migrate();
            Console.WriteLine("✅ Migrations applicate.");
            break;
        }
        catch (Exception ex)
        {
            retries++;
            Console.WriteLine($"⏳ Tentativo {retries}: {ex.Message}");
            if (retries >= 10) throw;
            Thread.Sleep(2000);
        }
    }
}

// ✅ Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🍸 CocktailImportService avviato su: " + builder.Configuration["ASPNETCORE_URLS"]);
app.Run();


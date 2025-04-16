using Microsoft.EntityFrameworkCore;
using UserProfileService.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Connessione al database PostgreSQL
builder.Services.AddDbContext<UserProfileDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔧 Aggiunta dei controller
builder.Services.AddControllers();

builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri("http://auth-service:80/");
});

// 🧪 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🛠 Esecuzione automatica delle migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserProfileDbContext>();
    db.Database.Migrate();
}

// 🌍 Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


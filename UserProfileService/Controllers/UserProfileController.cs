using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace UserProfileService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserProfileController(UserProfileDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] UserProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔍 Verifica che l'utente esista tramite AuthService
            var client = _httpClientFactory.CreateClient("AuthService");
            var response = await client.GetAsync($"/api/auth/exists/{request.UserId}");

            if (!response.IsSuccessStatusCode)
                return StatusCode(502, "Errore nella comunicazione con il servizio Auth");

            var json = await response.Content.ReadFromJsonAsync<Dictionary<string, bool>>();
            if (json?["exists"] != true)
                return BadRequest(new { error = "Utente inesistente" });

            var exists = await _context.UserProfiles.AnyAsync(up => up.UserId == request.UserId);
            if (exists)
                return Conflict("Esiste già un profilo per questo utente.");

            var profile = new UserProfile
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate.HasValue
                    ? DateTime.SpecifyKind(request.BirthDate.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                AlcoholAllowed = request.AlcoholAllowed,
                ConsentGdpr = request.ConsentGdpr,
                ConsentProfiling = request.ConsentProfiling
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfile), new { id = profile.UserId }, profile);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UserProfile updated)
        {
            if (id != updated.UserId)
                return BadRequest(new { error = "ID mismatch" });

            if (!await _context.UserProfiles.AnyAsync(p => p.UserId == id))
                return NotFound();

            if (updated.BirthDate.HasValue)
                updated.BirthDate = DateTime.SpecifyKind(updated.BirthDate.Value, DateTimeKind.Utc);

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
                return NotFound();

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


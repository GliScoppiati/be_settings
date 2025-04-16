using Microsoft.EntityFrameworkCore;
using UserProfileService.Models;

namespace UserProfileService.Models
{
    public class UserProfileDbContext : DbContext
    {
        public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace UserProfileService.Models
{
    public class UserProfileRequest
    {
        [Required]
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required]
        public bool ConsentGdpr { get; set; }

        public bool ConsentProfiling { get; set; }

        public bool AlcoholAllowed { get; set; }
    }
}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserProfileService.Models
{
    public class UserProfile
    {
        [Key]
        public Guid UserId { get; set; }  // stesso ID dell'utente (relazione 1-1 con Users)

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool AlcoholAllowed { get; set; }

        public bool ConsentGdpr { get; set; }

        public bool ConsentProfiling { get; set; }
    }
}


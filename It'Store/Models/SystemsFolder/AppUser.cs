using Microsoft.AspNetCore.Identity;

namespace ItStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }

        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? Flat { get; set; }

    }
}

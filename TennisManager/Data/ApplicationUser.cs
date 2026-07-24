using Microsoft.AspNetCore.Identity;

namespace TennisManager.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string? Club { get; set; }

        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";
    }

}

using Microsoft.AspNetCore.Identity;

namespace SharedProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

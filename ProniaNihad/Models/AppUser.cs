using Microsoft.AspNetCore.Identity;

namespace ProniaWebNihad.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
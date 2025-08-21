using Microsoft.AspNetCore.Identity;

namespace shop.Model
{
    public class AppUser : IdentityUser
    {
        public string UserLastName { get; set; } = string.Empty;
    }
}

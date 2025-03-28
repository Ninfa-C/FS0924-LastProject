using Microsoft.AspNetCore.Identity;

namespace LastProject.Models.Auth;

public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUserRole> UserRoles { get; set; }
}
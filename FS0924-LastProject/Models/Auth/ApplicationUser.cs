using System.ComponentModel.DataAnnotations;
using LastProject.Models.Main;
using Microsoft.AspNetCore.Identity;

namespace LastProject.Models.Auth;

public class ApplicationUser : IdentityUser
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Surname { get; set; }
    [Required]
    public bool IsDeleted { get; set; }

    public ICollection<ApplicationUserRole>? UserRoles { get; set; }
    public ICollection<Ticket>? Tickets { get; set; }

}
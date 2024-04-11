using Microsoft.AspNetCore.Identity;

namespace Intex2024.Models;

public class ApplicationRole : IdentityRole
{
    public string? RoleName { get; set; }  
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}
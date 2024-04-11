using Microsoft.AspNetCore.Identity;

namespace Intex2024.Models;

public class ApplicationUser : IdentityUser  
{  
    public int Age { get; set; }   
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }   
}

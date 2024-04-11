using Microsoft.AspNetCore.Identity;

namespace Intex2024.Models;
public class ApplicationUserRole : IdentityUserRole<string>  
{  
    public virtual ApplicationUser User { get; set; }  
    public virtual ApplicationRole Role { get; set; }  
} 
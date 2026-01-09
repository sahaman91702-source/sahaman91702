using System.Collections.Generic;
using JobPortal.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public ICollection<Company> Companies { get; set; } = new List<Company>();
}

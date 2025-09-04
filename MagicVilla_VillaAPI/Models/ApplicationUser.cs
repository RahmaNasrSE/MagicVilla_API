using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;

namespace MagicVilla_VillaAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

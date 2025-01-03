using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityRazor.Data.Account
{
    public class CustomUser: IdentityUser
    {
        [Required]
        public string Position {  get; set; } = string.Empty;
        [Required]
        public string Role {  get; set; } = string.Empty;
    }
}

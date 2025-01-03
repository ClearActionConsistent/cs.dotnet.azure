using AspNetCoreIdentityRazor.Data.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityRazor.Data
{
    public class IdentityDBContext : IdentityDbContext<CustomUser>
    {
        public IdentityDBContext(DbContextOptions<IdentityDBContext> options) : base(options)
        {

        }
    }
}

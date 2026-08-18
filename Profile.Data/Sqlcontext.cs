using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using profile;

namespace Profile.Data
{
    public class Sqlcontext :IdentityDbContext<Profilemodel>
    {
        public Sqlcontext(DbContextOptions<Sqlcontext> options) : base(options) { }
    }
}

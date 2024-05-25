using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Data
{
    public class ProjektContext : IdentityDbContext//<IdentityUser>
    {
        public ProjektContext(DbContextOptions<ProjektContext> options)
            : base(options)
        {
        }

        public DbSet<Projekt.Models.Task> Task { get; set; } = default!;
    }
}

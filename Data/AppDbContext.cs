using Microsoft.EntityFrameworkCore;
using RubenEjercicio.Models;

namespace RubenEjercicio.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
    }

}

using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using APIContaUsuario.Models;

namespace APIContaUsuario.Contexto
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<RolesUser> Roles { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}

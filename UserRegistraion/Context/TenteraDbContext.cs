using Microsoft.EntityFrameworkCore;
using UserRegistraion.Entities;

namespace UserRegistraion.Context
{
    public class TenteraDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Otp> Otps { get; set; }

        public TenteraDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //use this to configure the contex
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use this to configure the model
        }
    }
}

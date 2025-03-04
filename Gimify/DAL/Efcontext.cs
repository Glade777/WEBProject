using Gimify.DAL.Configurations;
using Gimify.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gimify.DAL
{
    public class Efcontext : DbContext
    {
        public Efcontext(DbContextOptions<Efcontext> options)
            : base(options)
        {    
        }

        public DbSet<Posts> Posts { get; set; }
        public DbSet<Exercises_Man> Exercises_Man { get; set; }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Favourite> Favourite { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new ManConfiguration());
            modelBuilder.ApplyConfiguration(new ExercisesConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FavouriteConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

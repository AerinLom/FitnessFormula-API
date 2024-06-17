using Microsoft.EntityFrameworkCore;
using FitnessFormula_API.Models;
namespace FitnessFormula_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Workout> Workout { get; set; }
        public DbSet<UserWorkout> UserWorkouts { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWorkout>()
                .HasKey(uw => new { uw.UserId, uw.WorkoutId });

            modelBuilder.Entity<UserWorkout>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.UserWorkouts)
                .HasForeignKey(uw => uw.UserId);

            modelBuilder.Entity<UserWorkout>()
                .HasOne(uw => uw.Workout)
                .WithMany(w => w.UserWorkouts)
                .HasForeignKey(uw => uw.WorkoutId);
        }
    }
}

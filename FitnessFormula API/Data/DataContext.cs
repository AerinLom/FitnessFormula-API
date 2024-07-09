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

        // Define DbSet properties for each entity
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Workout> Workout { get; set; }
        public DbSet<UserWorkout> UserWorkouts { get; set; }
        public DbSet<Exercises> Exercises { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship for UserWorkout entity
            modelBuilder.Entity<UserWorkout>()
                .HasKey(uw => new { uw.UserId, uw.WorkoutId }); // Composite key

            modelBuilder.Entity<UserWorkout>()
                .HasOne(uw => uw.User)
                .WithMany(u => u.UserWorkouts) // One user can have many user workouts
                .HasForeignKey(uw => uw.UserId); // Foreign key

            // Configure one-to-many relationship for Workout to Exercises
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Exercises) // One workout can have many exercises
                .WithOne(e => e.Workout) // Each exercise belongs to one workout
                .HasForeignKey(e => e.WorkoutId); // Foreign key in Exercises table

            // Configure auto-generated Id property for Exercises
            modelBuilder.Entity<Exercises>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd(); // Automatically generated on add

            base.OnModelCreating(modelBuilder); // Call base method for further configurations
        }
    }
}

using Microsoft.EntityFrameworkCore;

using RaceDayBackend.Models;

namespace RaceDayBackend.Data

{

    // The ": DbContext" here is what gives you access to Users, Roles, and SaveChangesAsync()

    public class RaceDayDBContext : DbContext

    {

        public RaceDayDBContext(DbContextOptions<RaceDayDBContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Enrolment> Enrolments { get; set; }

        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);

            // 1. Unique Constraints

            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();

            modelBuilder.Entity<User>().HasIndex(u => u.EmailAddress).IsUnique();

            // 2. Default Values

            modelBuilder.Entity<Sponsor>()

                .Property(s => s.SponsorshipTier)

                .HasDefaultValue("Bronze");

            modelBuilder.Entity<Enrolment>()

                .Property(e => e.Status)

                .HasDefaultValue("Confirmed");

            // 3. One-to-One Relationship (Enrolment to Result)

            modelBuilder.Entity<Enrolment>()

                .HasOne(e => e.Result)

                .WithOne(r => r.Enrolment)

                .HasForeignKey<Result>(r => r.EnrolmentID);

            // 4. Cascade Deletes (As per SQL Script)

            modelBuilder.Entity<Sponsor>()

                .HasOne(s => s.Event)

                .WithMany(e => e.Sponsors)

                .HasForeignKey(s => s.EventID)

                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()

                .HasOne(c => c.Event)

                .WithMany(e => e.Categories)

                .HasForeignKey(c => c.EventID)

                .OnDelete(DeleteBehavior.Cascade);

            // 5. Restrict deletes to prevent multiple cascade paths

            modelBuilder.Entity<Event>()

                .HasOne(e => e.Organiser)

                .WithMany(u => u.ManagedEvents)

                .HasForeignKey(e => e.OrganiserID)

                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()

                .HasOne(e => e.User)

                .WithMany(u => u.Enrolments)

                .HasForeignKey(e => e.UserID)

                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrolment>()

                .HasOne(e => e.Category)

                .WithMany(c => c.Enrolments)

                .HasForeignKey(e => e.CategoryID)

                .OnDelete(DeleteBehavior.Restrict);

        }

    }

}

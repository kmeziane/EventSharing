using EventSharing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EventSharing.ViewModels;

namespace EventSharing.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event>? Events { get; set; }
        public DbSet<Category>? Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasMany(u => u.CreatdEvents)
                .WithOne(e => e.Creator);

            builder.Entity<Event>()
                .HasOne(e => e.Creator)
                .WithMany(u => u.CreatdEvents);

            builder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Category>()
                .Property(c => c.Description)
                .IsRequired();

            builder.Entity<Event>()
                .Property(e => e.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Entity<Event>()
                .Property(e => e.Description)
                .IsRequired();

            builder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(256);
        }


        }
}
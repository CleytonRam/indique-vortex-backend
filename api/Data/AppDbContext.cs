using Microsoft.EntityFrameworkCore;
using ReferralApi.Models;


namespace ReferralApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.email)
                .IsUnique();
                entity.HasIndex(u => u.refCode)
                .IsUnique();
                entity.Property(u => u.name)
                .HasMaxLength(100)
                .IsRequired();
                entity.Property(u => u.email)
                .HasMaxLength(100)
                .IsRequired();
                entity.Property(u => u.email)
                .HasMaxLength(255)
                .IsRequired();
                entity.Property(u => u.passwordHash)
                .IsRequired();
                entity.Property(u => u.refCode)
                .HasMaxLength(8)
                .IsRequired();
                entity.HasOne(u => u.referredBy)
                .WithMany()
                .HasForeignKey(u => u.referredById)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

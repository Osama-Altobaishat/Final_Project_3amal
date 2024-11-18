using Final_Project_3amal.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_3amal.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ImageService> ImageServices { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            // Relationships
            modelBuilder.Entity<Service>()
                .HasOne(s => s.User)  // Service has one User
                .WithMany(u => u.Services) // User has many Services
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Service>()
                .HasOne(s => s.Category)  // Service has one Category
                .WithMany(c => c.Services) // Category has many Services
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Service)  // Review has one Service
                .WithMany(s => s.Reviews) // Service has many Reviews
                .HasForeignKey(r => r.ServiceId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)  // Review has one User
                .WithMany(u => u.Reviews) // User has many Reviews
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Testimonial>()
                .HasOne(t => t.User)  // Testimonial has one User
                .WithMany(u => u.Testimonials) // User has many Testimonials
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.User)  // ServiceRequest has one User
                .WithMany(u => u.ServiceRequests) // User has many ServiceRequests
                .HasForeignKey(sr => sr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Service)  // ServiceRequest has one Service
                .WithMany(s => s.ServiceRequests) // Service has many ServiceRequests
                .HasForeignKey(sr => sr.ServiceId);


            modelBuilder.Entity<ImageService>()
              .HasOne(sr => sr.Service)  // ServiceRequest has one Service
              .WithMany(s => s.ImageService) // Service has many ServiceRequests
              .HasForeignKey(sr => sr.ServiceId);
            

            base.OnModelCreating(modelBuilder);
        }
    }
}

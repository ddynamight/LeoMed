using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LeoMed.Model
{
     public class AppDbContext : IdentityDbContext<AppUser>
     {
          public AppDbContext()
          {

          }

          public AppDbContext(DbContextOptions options) : base(options)
          {
               
          }

          public static AppDbContext Create()
          {
               return new AppDbContext();
          }

          public virtual DbSet<AppUser> AppUsers { get; set; }
          public virtual DbSet<Blog> Blogs { get; set; }
          public virtual DbSet<History> Histories { get; set; }
          public virtual DbSet<News> News { get; set; }
          public virtual DbSet<Patient> Patients { get; set; }
          public virtual DbSet<Professional> Professionals { get; set; }

          protected override void OnModelCreating(ModelBuilder builder)
          {
               // Single Property Configurations
               builder.Entity<AppUser>().HasKey(e => e.Id);
               builder.Entity<Blog>().HasKey(e => e.Id);
               builder.Entity<News>().HasKey(e => e.Id);
               builder.Entity<Patient>().HasKey(e => e.AppUserId);
               builder.Entity<Professional>().HasKey(e => e.AppUserId);
               builder.Entity<IdentityUserLogin<string>>().HasKey(e => e.UserId);
               builder.Entity<IdentityUserRole<string>>().HasKey(e => new {e.UserId, e.RoleId });
               builder.Entity<IdentityRole<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityRoleClaim<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityUserClaim<string>>().HasKey(e => e.Id);
               builder.Entity<IdentityUserToken<string>>().HasKey(e => e.UserId);

               // ToTable Property Configuration
               builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogin");
               builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRole");
               builder.Entity<IdentityRole<string>>().ToTable("AspNetRole");
               builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaim");
               builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaim");
               builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserToken");


               // One to Many Relationship Configuration
               builder.Entity<Patient>()
                    .HasMany(p => p.Histories)
                    .WithOne(h => h.Patient)
                    .HasForeignKey(h => h.PatientAppUserId);

               builder.Entity<Professional>()
                    .HasMany(p => p.Histories)
                    .WithOne(h => h.Professional)
                    .HasForeignKey(h => h.ProfessionalAppUserId);


               // One to Zero One Relationship Configuration
               builder.Entity<AppUser>()
                    .HasOne(a => a.Patient)
                    .WithOne(p => p.AppUser);

               builder.Entity<AppUser>()
                    .HasOne(a => a.Professional)
                    .WithOne(p => p.AppUser);



          }

          protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
               optionsBuilder.UseSqlServer("Server=.;Database=LeoMed;Trusted_Connection=True;MultipleActiveResultSets=true");
          }
     }
}

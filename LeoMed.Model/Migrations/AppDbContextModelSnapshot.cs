using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using LeoMed.Model;

namespace LeoMed.Model.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LeoMed.Model.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Middlename");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Sex");

                    b.Property<string>("Title");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("LeoMed.Model.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Status");

                    b.Property<string>("Tagname");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("LeoMed.Model.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Details");

                    b.Property<string>("PatientAppUserId");

                    b.Property<string>("ProfessionalAppUserId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("PatientAppUserId");

                    b.HasIndex("ProfessionalAppUserId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("LeoMed.Model.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Article");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Status");

                    b.Property<string>("Tagname");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("LeoMed.Model.Patient", b =>
                {
                    b.Property<string>("AppUserId");

                    b.Property<string>("Country");

                    b.Property<string>("Location");

                    b.Property<string>("PatientNo");

                    b.Property<string>("State");

                    b.HasKey("AppUserId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("LeoMed.Model.Professional", b =>
                {
                    b.Property<string>("AppUserId");

                    b.Property<string>("Description");

                    b.Property<int>("Experience");

                    b.Property<string>("Profession");

                    b.HasKey("AppUserId");

                    b.ToTable("Professionals");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<string>", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.HasKey("Id");

                    b.ToTable("AspNetRole");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole<string>");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("IdentityRoleId");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("IdentityRoleId");

                    b.ToTable("AspNetRoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("AspNetUserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.HasKey("UserId");

                    b.HasIndex("AppUserId");

                    b.ToTable("AspNetUserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("AppUserId");

                    b.Property<string>("IdentityRoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("IdentityRoleId");

                    b.ToTable("AspNetUserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.ToTable("AspNetUserToken");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole<string>");


                    b.ToTable("IdentityRole");

                    b.HasDiscriminator().HasValue("IdentityRole");
                });

            modelBuilder.Entity("LeoMed.Model.History", b =>
                {
                    b.HasOne("LeoMed.Model.Patient", "Patient")
                        .WithMany("Histories")
                        .HasForeignKey("PatientAppUserId");

                    b.HasOne("LeoMed.Model.Professional", "Professional")
                        .WithMany("Histories")
                        .HasForeignKey("ProfessionalAppUserId");
                });

            modelBuilder.Entity("LeoMed.Model.Patient", b =>
                {
                    b.HasOne("LeoMed.Model.AppUser", "AppUser")
                        .WithOne("Patient")
                        .HasForeignKey("LeoMed.Model.Patient", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LeoMed.Model.Professional", b =>
                {
                    b.HasOne("LeoMed.Model.AppUser", "AppUser")
                        .WithOne("Professional")
                        .HasForeignKey("LeoMed.Model.Professional", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("IdentityRoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LeoMed.Model.AppUser")
                        .WithMany("Claims")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LeoMed.Model.AppUser")
                        .WithMany("Logins")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("LeoMed.Model.AppUser")
                        .WithMany("Roles")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("IdentityRoleId");
                });
        }
    }
}

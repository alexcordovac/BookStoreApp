using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Data;


public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Authors_Id");

            entity.Property(e => e.Bio).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Books_Id");

            entity.HasIndex(e => e.Isbn, "IX_Books_ISBN").IsUnique();

            entity.Property(e => e.Isbn).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Summary).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Author).WithMany(p => p.Books).HasForeignKey(d => d.AuthorId);
        });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c"
            },
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"
            }
        );

        var hasher = new PasswordHasher<ApiUser>();
        modelBuilder.Entity<ApiUser>().HasData(
            new ApiUser
            {
                Id = "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g",
                Email = "admin@bookstore.com",
                NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                UserName = "admin@bookstore.com",
                NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "Administrator",
                PasswordHash = "AQAAAAIAAYagAAAAEAuyFsll6VYAeMEaVLQTenqT+u0SQpXWrdksYDexdoqVucArAURL7RG2IJ1gdMongA==",//12345
                SecurityStamp = "019a2c8f-fd1c-7b4e-b452-19fcc6e79d63",
                ConcurrencyStamp = "019a2c94-4b3c-79d5-8e39-750e62e5b36e"
            },
            new ApiUser
            {
                Id = "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d",
                Email = "user@bookstore.com",
                NormalizedEmail = "USER@BOOKSTORE.COM",
                UserName = "user@bookstore.com",
                NormalizedUserName = "USER@BOOKSTORE.COM",
                FirstName = "System",
                LastName = "User",
                PasswordHash = "AQAAAAIAAYagAAAAEAuyFsll6VYAeMEaVLQTenqT+u0SQpXWrdksYDexdoqVucArAURL7RG2IJ1gdMongA==",//12345
                SecurityStamp = "019a2c90-1cbb-7452-907e-4e85502bcc51",
                ConcurrencyStamp = "019a2c94-7929-75f7-a585-b0fe0ea4737d"
            }
        );


        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
                UserId = "d1e2f3a4-b5c6-7d8e-9f0a-1b2c3d4e5f6g"
            },
            new IdentityUserRole<string>
            {
                RoleId = "8f6f2b4e-4d2a-4c3a-9c0e-1f2e5d6a7b8c",
                UserId = "a1b2c3d4-e5f6-7a8b-9c0d-1e2f3a4b5c6d"
            }
        );


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

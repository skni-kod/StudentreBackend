using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StudentreBackend.Data.Models;

namespace StudentreBackend.Data
{
    public partial class BaseDbContext : DbContext
    {
        public BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null!;
        public virtual DbSet<VersionInfo> VersionInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

				optionsBuilder.UseNpgsql("DefaultDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Value).HasMaxLength(32);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "IX_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "IX_User_Login")
                    .IsUnique();

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.College).HasMaxLength(128);

                entity.Property(e => e.DateCreatedUtc).HasColumnType("timestamp without time zone");

                entity.Property(e => e.DateDeletedUtc).HasColumnType("timestamp without time zone");

                entity.Property(e => e.DateModifiedUtc).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Department).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(64);

                entity.Property(e => e.FieldOfStudy).HasMaxLength(64);

                entity.Property(e => e.FirstName).HasMaxLength(32);

                entity.Property(e => e.LastLogin).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastName).HasMaxLength(32);

                entity.Property(e => e.Login).HasMaxLength(64);

                entity.Property(e => e.Password).HasMaxLength(256);

                entity.Property(e => e.Photo).HasMaxLength(512);

                entity.Property(e => e.PublicId)
                    .HasMaxLength(36)
                    .IsFixedLength();

                entity.Property(e => e.RefreshToken).HasMaxLength(256);

                entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("timestamp without time zone");

                entity.Property(e => e.StudentId).HasMaxLength(32);

                entity.Property(e => e.Term).HasMaxLength(32);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_RoleId");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("UserGroup");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_GroupId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserId");
            });

            modelBuilder.Entity<VersionInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VersionInfo");

                entity.HasIndex(e => e.Version, "UC_Version")
                    .IsUnique();

                entity.Property(e => e.AppliedOn).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Description).HasMaxLength(1024);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

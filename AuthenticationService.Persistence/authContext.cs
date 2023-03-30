using System;
using System.Collections.Generic;
using AuthenticationService.Domain.Models;
using AuthenticationService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AuthenticationService.Persistence
{
    public partial class authContext : DbContext
    {
        public authContext()
        {
        }

        public authContext(DbContextOptions<authContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=auth;Username=postgres;Password=mysecretpassword");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Id, "roles_\"id\"_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "roles_\"name\"_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "users_\"email\"_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "users_\"id\"_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.KeycloakId, "users_\"keycloackid\"_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Users_id_seq\"'::regclass)");

                entity.Property(e => e.Name).HasColumnType("character varying");

                entity.Property(e => e.RoleId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("users_role_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

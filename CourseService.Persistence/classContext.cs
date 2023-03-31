using System;
using System.Collections.Generic;
using CourseService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CourseService.Persistence.Models
{
    public partial class classContext : DbContext
    {
        public classContext()
        {
        }

        public classContext(DbContextOptions<classContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Tenant> Tenants { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=class;Username=postgres;Password=mysecretpassword");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.Name, "courses_\"name\"_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Courses_Id_seq1\"'::regclass)");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("courses_tenant_id_fk");
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.ToTable("Tenant");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.HasSequence("Class_CourseId_seq");

            modelBuilder.HasSequence("ClassEnrollment_Id_seq");

            modelBuilder.HasSequence("Courses_Id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

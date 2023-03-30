using EnrollementService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollementService.Persistence
{
    public partial class classenrollmentContext : DbContext
    {
        public classenrollmentContext()
        {
        }

        public classenrollmentContext(DbContextOptions<classenrollmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClassEnrollment> ClassEnrollments { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<TeacherPerCourse> TeacherPerCourses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=classenrollment;Username=postgres;Password=mysecretpassword");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassEnrollment>(entity =>
            {
                entity.ToTable("ClassEnrollment");

                entity.HasIndex(e => e.Id, "classenrollment_id_uindex")
                    .IsUnique();

                entity.Property(e => e.ClassId).ValueGeneratedOnAdd();

                entity.Property(e => e.StudentId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassEnrollments)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("classenrollment_class_id_fk");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ClassEnrollments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("classenrollment_users_id_fk");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.Name, "courses_\"name\"_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "courses_id_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Id, "roles_\"id\"_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "roles_\"name\"_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<TeacherPerCourse>(entity =>
            {
                entity.ToTable("TeacherPerCourse");

                entity.HasIndex(e => e.Id, "class_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Class_Id_seq\"'::regclass)");

                entity.Property(e => e.CourseId).HasDefaultValueSql("nextval('\"Class_CourseId_seq\"'::regclass)");

                entity.Property(e => e.TeacherId).HasDefaultValueSql("nextval('\"Class_TeacherId_seq\"'::regclass)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.TeacherPerCourses)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("class_course_id_fk");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.TeacherPerCourses)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("class_teacher_id_fk");
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

            modelBuilder.HasSequence("ClassSessions_Id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Prog6212Poe.Models
{
    public partial class TimeWizContext : DbContext
    {
        public TimeWizContext()
        {
        }

        public TimeWizContext(DbContextOptions<TimeWizContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<ModuleTable> ModuleTables { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.LoginId).HasColumnName("Login_Id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ModuleTable>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("PK__ModuleTa__1DE4E0C80B944AB5");

                entity.ToTable("ModuleTable");

                entity.Property(e => e.ModuleId).HasColumnName("Module_Id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.SemesterId).HasColumnName("Semester_Id");

                entity.Property(e => e.StudyDate).HasColumnType("datetime");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.ModuleTables)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModuleTable_Semester");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_Id");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Semesters)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Semester_Student");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginId).HasColumnName("Login_Id");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Surname).IsUnicode(false);

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK_Student_Login");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IscConcursLr1
{
    public partial class DBIsConcursContext : DbContext
    {
        public DBIsConcursContext()
        {
        }

        public DBIsConcursContext(DbContextOptions<DBIsConcursContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Faculties> Faculties { get; set; }
        public virtual DbSet<FacultiesSpecialties> FacultiesSpecialties { get; set; }
        public virtual DbSet<Specialties> Specialties { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Universities> Universities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server = DESKTOP-6DUPNVC\\SQLEXPRESS; Database = DBIsConcurs; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculties>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode()
                    .IsFixedLength();

                entity.HasOne(d => d.FacultiesUniversityNavigation)
                    .WithMany(p => p.Faculties)
                    .HasForeignKey(d => d.FacultiesUniversity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Faculties_Universities");
            });
           
            modelBuilder.Entity<FacultiesSpecialties>(entity =>
            {
                entity.Property(e => e.FacultiesSpecialtiesId).HasColumnName("FacultiesSpecialties_Id");

                entity.Property(e => e.FsFaculties).HasColumnName("Fs_Faculties");

                entity.Property(e => e.FsSpecialties).HasColumnName("Fs_Specialties");

                entity.HasOne(d => d.FsFacultiesNavigation)
                    .WithMany(p => p.FacultiesSpecialties)
                    .HasForeignKey(d => d.FsFaculties)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultiesSpecialties_Faculties");

                entity.HasOne(d => d.FsSpecialtiesNavigation)
                    .WithMany(p => p.FacultiesSpecialties)
                    .HasForeignKey(d => d.FsSpecialties)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacultiesSpecialties_Specialties");
            });

            modelBuilder.Entity<Specialties>(entity =>
            {
                entity.Property(e => e.SpecialtiesId).HasColumnName("Specialties_Id");

                entity.Property(e => e.Info).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Statements>(entity =>
            {
                entity.HasKey(e => e.StatementId);

                entity.Property(e => e.StatementId).HasColumnName("Statement_Id");

                entity.Property(e => e.StFacultiesSpecialties).HasColumnName("St_FacultiesSpecialties");

                entity.Property(e => e.StStudent).HasColumnName("St_Student");

                entity.HasOne(d => d.StFacultiesSpecialtiesNavigation)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.StFacultiesSpecialties)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statements_FacultiesSpecialties");

                entity.HasOne(d => d.StStudentNavigation)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.StStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statements_Students");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.StudentId).HasColumnName("Student_Id");

                entity.Property(e => e.Info)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Universities>(entity =>
            {
                entity.HasKey(e => e.UniversityId);

                entity.Property(e => e.UniversityId).HasColumnName("University_Id");

                entity.Property(e => e.Info)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

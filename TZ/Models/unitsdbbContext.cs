using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TZ.Models
{
    public partial class unitsdbbContext : DbContext
    {
        public unitsdbbContext()
        {
        }

        public unitsdbbContext(DbContextOptions<unitsdbbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Unit> Units { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=unitsdbb;Username=postgres;Password=1234;Include Error Detail=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("unit_pkey");

                entity.ToTable("unit");

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .HasColumnName("name");

                entity.Property(e => e.Parentname)
                    .HasMaxLength(300)
                    .HasColumnName("parentname");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.ParentnameNavigation)
                    .WithMany(p => p.InverseParentnameNavigation)
                    .HasForeignKey(d => d.Parentname)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("unit_parentname_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

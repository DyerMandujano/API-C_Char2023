using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NetCoreProyect.Models
{
    public partial class BD_Empresa2023Context : DbContext
    {
        public BD_Empresa2023Context()
        {
        }

        public BD_Empresa2023Context(DbContextOptions<BD_Empresa2023Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<Distrito> Distritos { get; set; } = null!;
        public virtual DbSet<Empleado> Empleados { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.Idcargo)
                    .HasName("PK__Cargo__0515A5ADF921D841");

                entity.ToTable("Cargo");

                entity.Property(e => e.Idcargo).HasColumnName("idcargo");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NomCargo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nom_cargo");

                entity.Property(e => e.Sueldo)
                    .HasColumnType("money")
                    .HasColumnName("sueldo");
            });

            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.HasKey(e => e.Iddist)
                    .HasName("PK__Distrito__CEEBE8930EB37F73");

                entity.ToTable("Distrito");

                entity.Property(e => e.Iddist).HasColumnName("iddist");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NomDist)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nom_dist");
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Idemp)
                    .HasName("PK__Empleado__07D8CFBD607CCDB9");

                entity.ToTable("Empleado");

                entity.Property(e => e.Idemp).HasColumnName("idemp");

                entity.Property(e => e.ApeEmp)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ape_emp");

                entity.Property(e => e.Correo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Idcargo).HasColumnName("idcargo");

                entity.Property(e => e.Iddist).HasColumnName("iddist");

                entity.Property(e => e.NomEmp)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("nom_emp");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("telefono")
                    .IsFixedLength();

                entity.HasOne(d => d.IdcargoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.Idcargo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cargo");

                entity.HasOne(d => d.IddistNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.Iddist)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_distrito");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entidades.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campo> Campos { get; set; } = null!;
        public virtual DbSet<Entidade> Entidades { get; set; } = null!;
        public virtual DbSet<Muestra> Muestras { get; set; } = null!;
        public virtual DbSet<NombresDatosEstatico> NombresDatosEstaticos { get; set; } = null!;
        public virtual DbSet<NombresVariablesMuestra> NombresVariablesMuestras { get; set; } = null!;
        public virtual DbSet<TiposMuestra> TiposMuestras { get; set; } = null!;
        public virtual DbSet<ValoresDatosEstatico> ValoresDatosEstaticos { get; set; } = null!;
        public virtual DbSet<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                //optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Entidades;User Id=sa;Password=juan;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campo>(entity =>
            {
                entity.ToTable("campos");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Entidade>(entity =>
            {
                entity.ToTable("entidades");

                entity.Property(e => e.Id)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAlta");
            });

            modelBuilder.Entity<Muestra>(entity =>
            {
                entity.ToTable("muestras");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdCampo).HasColumnName("idCampo");

                entity.Property(e => e.IdEntidad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("idEntidad");

                entity.Property(e => e.IdTipoMuestra).HasColumnName("idTipoMuestra");

                entity.HasOne(d => d.IdCampoNavigation)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.IdCampo)
                    .HasConstraintName("FK__muestras__idCamp__76969D2E");

                entity.HasOne(d => d.IdEntidadNavigation)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.IdEntidad)
                    .HasConstraintName("FK__muestras__idEnti__74AE54BC");

                entity.HasOne(d => d.IdTipoMuestraNavigation)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.IdTipoMuestra)
                    .HasConstraintName("FK__muestras__idTipo__75A278F5");
            });

            modelBuilder.Entity<NombresDatosEstatico>(entity =>
            {
                entity.ToTable("nombresDatosEstaticos");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<NombresVariablesMuestra>(entity =>
            {
                entity.ToTable("nombresVariablesMuestras");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdTipoMuestra).HasColumnName("idTipoMuestra");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdTipoMuestraNavigation)
                    .WithMany(p => p.NombresVariablesMuestras)
                    .HasForeignKey(d => d.IdTipoMuestra)
                    .HasConstraintName("FK__nombresVa__idTip__66603565");
            });

            modelBuilder.Entity<TiposMuestra>(entity =>
            {
                entity.ToTable("tiposMuestras");

                entity.HasIndex(e => e.Nombre, "UQ__tiposMue__72AFBCC6620C8E8F")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<ValoresDatosEstatico>(entity =>
            {
                entity.ToTable("valoresDatosEstaticos");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdEntidad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("idEntidad");

                entity.Property(e => e.IdNombreDatoEstatico).HasColumnName("idNombreDatoEstatico");

                entity.Property(e => e.Valor)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("valor");

                entity.HasOne(d => d.IdEntidadNavigation)
                    .WithMany(p => p.ValoresDatosEstaticos)
                    .HasForeignKey(d => d.IdEntidad)
                    .HasConstraintName("FK__valoresDa__idEnt__6FE99F9F");

                entity.HasOne(d => d.IdNombreDatoEstaticoNavigation)
                    .WithMany(p => p.ValoresDatosEstaticos)
                    .HasForeignKey(d => d.IdNombreDatoEstatico)
                    .HasConstraintName("FK__valoresDa__idNom__70DDC3D8");
            });

            modelBuilder.Entity<ValoresVariablesMuestra>(entity =>
            {
                entity.ToTable("valoresVariablesMuestras");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdMuestra).HasColumnName("idMuestra");

                entity.Property(e => e.IdNombreVariableMuestra).HasColumnName("idNombreVariableMuestra");

                entity.Property(e => e.Valor)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("valor");

                entity.HasOne(d => d.IdMuestraNavigation)
                    .WithMany(p => p.ValoresVariablesMuestras)
                    .HasForeignKey(d => d.IdMuestra)
                    .HasConstraintName("FK__valoresVa__idMue__7B5B524B");

                entity.HasOne(d => d.NombreVariableMuestraNavigation)
                    .WithMany(p => p.ValoresVariablesMuestras)
                    .HasForeignKey(d => d.IdNombreVariableMuestra)
                    .HasConstraintName("FK__valoresVa__idNom__7A672E12");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

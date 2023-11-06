using System;
using System.Collections.Generic;
using Entidades.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entidades.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString: "Filename=Entidades.db",
                  options =>
                  {
                      options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                      //options.GetExecutingAssembly().FullName;
                  }

                  );

            base.OnConfiguring(optionsBuilder);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Campo> Campos { get; set; } = null!;
        public virtual DbSet<Entidade> Entidades { get; set; } = null!;
        public virtual DbSet<Muestra> Muestras { get; set; } = null!;
        public virtual DbSet<NombresDatosEstatico> NombresDatosEstaticos { get; set; } = null!;
        public virtual DbSet<NombresVariablesMuestra> NombresVariablesMuestras { get; set; } = null!;
        public virtual DbSet<TiposMuestra> TiposMuestras { get; set; } = null!;
        public virtual DbSet<ValoresDatosEstatico> ValoresDatosEstaticos { get; set; } = null!;
        public virtual DbSet<ValoresVariablesMuestra> ValoresVariablesMuestras { get; set; } = null!;
        public virtual DbSet<ValoresReferencia> ValoresReferencia { get; set; } = null!;



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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_campos_muestras");

                entity.HasOne(d => d.IdEntidadNavigation)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.IdEntidad)
                    .HasConstraintName("FK_entidades_muestras");

                entity.HasOne(d => d.IdTipoMuestraNavigation)
                    .WithMany(p => p.Muestras)
                    .HasForeignKey(d => d.IdTipoMuestra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tiposMuestras_muestras");
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tiposMuestras_nombresVariablesMuestras");
            });

            modelBuilder.Entity<TiposMuestra>(entity =>
            {
                entity.ToTable("tiposMuestras");

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
                    .HasConstraintName("FK_nombresDatosEstaticos_valoresDatosEstaticos");

                entity.HasOne(d => d.IdNombreDatoEstaticoNavigation)
                    .WithMany(p => p.ValoresDatosEstaticos)
                    .HasForeignKey(d => d.IdNombreDatoEstatico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_entidades_valoresDatosEstaticos");
            });

            modelBuilder.Entity<ValoresReferencia>(entity =>
            {
                entity.ToTable("valoresReferencia");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdNombreVariableMuestra).HasColumnName("idNombreVariableMuestra");

                entity.Property(e => e.Maximo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("maximo");

                entity.Property(e => e.Minimo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("minimo");

                entity.HasOne(d => d.NombreVariableMuestra)
                    .WithMany(p => p.ValoresReferencia)
                    .HasForeignKey(d => d.IdNombreVariableMuestra)
                    .HasConstraintName("FK_nombresVariablesMuestra_valoresReferencia");
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
                    .HasConstraintName("FK_muestras_valoresVariablesMuestras");

                entity.HasOne(d => d.IdNombreVariableMuestraNavigation)
                    .WithMany(p => p.ValoresVariablesMuestras)
                    .HasForeignKey(d => d.IdNombreVariableMuestra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nombresVariablesMuestra_valoresVariablesMuestras");
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

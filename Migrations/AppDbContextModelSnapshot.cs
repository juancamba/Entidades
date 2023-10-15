﻿// <auto-generated />
using System;
using Entidades.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entidades.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entidades.Models.Campo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.ToTable("campos", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.Entidade", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAlta");

                    b.HasKey("Id");

                    b.ToTable("entidades", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.Muestra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime")
                        .HasColumnName("fecha");

                    b.Property<int?>("IdCampo")
                        .HasColumnType("int")
                        .HasColumnName("idCampo");

                    b.Property<string>("IdEntidad")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("idEntidad");

                    b.Property<int?>("IdTipoMuestra")
                        .HasColumnType("int")
                        .HasColumnName("idTipoMuestra");

                    b.HasKey("Id");

                    b.HasIndex("IdCampo");

                    b.HasIndex("IdEntidad");

                    b.HasIndex("IdTipoMuestra");

                    b.ToTable("muestras", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.NombresDatosEstatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.ToTable("nombresDatosEstaticos", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.NombresVariablesMuestra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("IdTipoMuestra")
                        .HasColumnType("int")
                        .HasColumnName("idTipoMuestra");

                    b.Property<string>("Nombre")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.HasIndex("IdTipoMuestra");

                    b.ToTable("nombresVariablesMuestras", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.TiposMuestra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nombre");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Nombre" }, "UQ__tiposMue__72AFBCC6620C8E8F")
                        .IsUnique()
                        .HasFilter("[nombre] IS NOT NULL");

                    b.ToTable("tiposMuestras", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.ValoresDatosEstatico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("IdEntidad")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("idEntidad");

                    b.Property<int?>("IdNombreDatoEstatico")
                        .HasColumnType("int")
                        .HasColumnName("idNombreDatoEstatico");

                    b.Property<string>("Valor")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("IdEntidad");

                    b.HasIndex("IdNombreDatoEstatico");

                    b.ToTable("valoresDatosEstaticos", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.ValoresReferencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("IdNombreVariableMuestra")
                        .HasColumnType("int")
                        .HasColumnName("idNombreVariableMuestra");

                    b.Property<string>("Maximo")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("maximo");

                    b.Property<string>("Minimo")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("minimo");

                    b.HasKey("Id");

                    b.HasIndex("IdNombreVariableMuestra")
                        .IsUnique()
                        .HasFilter("[idNombreVariableMuestra] IS NOT NULL");

                    b.ToTable("valoresReferencia", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.ValoresVariablesMuestra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("IdMuestra")
                        .HasColumnType("int")
                        .HasColumnName("idMuestra");

                    b.Property<int?>("IdNombreVariableMuestra")
                        .HasColumnType("int")
                        .HasColumnName("idNombreVariableMuestra");

                    b.Property<string>("Valor")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("IdMuestra");

                    b.HasIndex("IdNombreVariableMuestra");

                    b.ToTable("valoresVariablesMuestras", (string)null);
                });

            modelBuilder.Entity("Entidades.Models.Muestra", b =>
                {
                    b.HasOne("Entidades.Models.Campo", "IdCampoNavigation")
                        .WithMany("Muestras")
                        .HasForeignKey("IdCampo")
                        .HasConstraintName("FK__muestras__idCamp__76969D2E");

                    b.HasOne("Entidades.Models.Entidade", "IdEntidadNavigation")
                        .WithMany("Muestras")
                        .HasForeignKey("IdEntidad")
                        .HasConstraintName("FK__muestras__idEnti__74AE54BC");

                    b.HasOne("Entidades.Models.TiposMuestra", "IdTipoMuestraNavigation")
                        .WithMany("Muestras")
                        .HasForeignKey("IdTipoMuestra")
                        .HasConstraintName("FK__muestras__idTipo__75A278F5");

                    b.Navigation("IdCampoNavigation");

                    b.Navigation("IdEntidadNavigation");

                    b.Navigation("IdTipoMuestraNavigation");
                });

            modelBuilder.Entity("Entidades.Models.NombresVariablesMuestra", b =>
                {
                    b.HasOne("Entidades.Models.TiposMuestra", "IdTipoMuestraNavigation")
                        .WithMany("NombresVariablesMuestras")
                        .HasForeignKey("IdTipoMuestra")
                        .HasConstraintName("FK__nombresVa__idTip__66603565");

                    b.Navigation("IdTipoMuestraNavigation");
                });

            modelBuilder.Entity("Entidades.Models.ValoresDatosEstatico", b =>
                {
                    b.HasOne("Entidades.Models.Entidade", "IdEntidadNavigation")
                        .WithMany("ValoresDatosEstaticos")
                        .HasForeignKey("IdEntidad")
                        .HasConstraintName("FK__valoresDa__idEnt__6FE99F9F");

                    b.HasOne("Entidades.Models.NombresDatosEstatico", "IdNombreDatoEstaticoNavigation")
                        .WithMany("ValoresDatosEstaticos")
                        .HasForeignKey("IdNombreDatoEstatico")
                        .HasConstraintName("FK__valoresDa__idNom__70DDC3D8");

                    b.Navigation("IdEntidadNavigation");

                    b.Navigation("IdNombreDatoEstaticoNavigation");
                });

            modelBuilder.Entity("Entidades.Models.ValoresReferencia", b =>
                {
                    b.HasOne("Entidades.Models.NombresVariablesMuestra", "NombreVariableMuestraNavigation")
                        .WithOne("ValoresReferencia")
                        .HasForeignKey("Entidades.Models.ValoresReferencia", "IdNombreVariableMuestra")
                        .HasConstraintName("FK__valoresRe__idNom__7D439ABD");

                    b.Navigation("NombreVariableMuestraNavigation");
                });

            modelBuilder.Entity("Entidades.Models.ValoresVariablesMuestra", b =>
                {
                    b.HasOne("Entidades.Models.Muestra", "IdMuestraNavigation")
                        .WithMany("ValoresVariablesMuestras")
                        .HasForeignKey("IdMuestra")
                        .HasConstraintName("FK__valoresVa__idMue__7B5B524B");

                    b.HasOne("Entidades.Models.NombresVariablesMuestra", "NombreVariableMuestraNavigation")
                        .WithMany("ValoresVariablesMuestras")
                        .HasForeignKey("IdNombreVariableMuestra")
                        .HasConstraintName("FK__valoresVa__idNom__7A672E12");

                    b.Navigation("IdMuestraNavigation");

                    b.Navigation("NombreVariableMuestraNavigation");
                });

            modelBuilder.Entity("Entidades.Models.Campo", b =>
                {
                    b.Navigation("Muestras");
                });

            modelBuilder.Entity("Entidades.Models.Entidade", b =>
                {
                    b.Navigation("Muestras");

                    b.Navigation("ValoresDatosEstaticos");
                });

            modelBuilder.Entity("Entidades.Models.Muestra", b =>
                {
                    b.Navigation("ValoresVariablesMuestras");
                });

            modelBuilder.Entity("Entidades.Models.NombresDatosEstatico", b =>
                {
                    b.Navigation("ValoresDatosEstaticos");
                });

            modelBuilder.Entity("Entidades.Models.NombresVariablesMuestra", b =>
                {
                    b.Navigation("ValoresReferencia");

                    b.Navigation("ValoresVariablesMuestras");
                });

            modelBuilder.Entity("Entidades.Models.TiposMuestra", b =>
                {
                    b.Navigation("Muestras");

                    b.Navigation("NombresVariablesMuestras");
                });
#pragma warning restore 612, 618
        }
    }
}

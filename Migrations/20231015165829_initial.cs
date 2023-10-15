using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entidades.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entidades",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    fechaAlta = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entidades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nombresDatosEstaticos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nombresDatosEstaticos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tiposMuestras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiposMuestras", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valoresDatosEstaticos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    valor = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    idEntidad = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    idNombreDatoEstatico = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valoresDatosEstaticos", x => x.id);
                    table.ForeignKey(
                        name: "FK__valoresDa__idEnt__6FE99F9F",
                        column: x => x.idEntidad,
                        principalTable: "entidades",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__valoresDa__idNom__70DDC3D8",
                        column: x => x.idNombreDatoEstatico,
                        principalTable: "nombresDatosEstaticos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "muestras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEntidad = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    idTipoMuestra = table.Column<int>(type: "int", nullable: true),
                    idCampo = table.Column<int>(type: "int", nullable: true),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_muestras", x => x.id);
                    table.ForeignKey(
                        name: "FK__muestras__idCamp__76969D2E",
                        column: x => x.idCampo,
                        principalTable: "campos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__muestras__idEnti__74AE54BC",
                        column: x => x.idEntidad,
                        principalTable: "entidades",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__muestras__idTipo__75A278F5",
                        column: x => x.idTipoMuestra,
                        principalTable: "tiposMuestras",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "nombresVariablesMuestras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    idTipoMuestra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nombresVariablesMuestras", x => x.id);
                    table.ForeignKey(
                        name: "FK__nombresVa__idTip__66603565",
                        column: x => x.idTipoMuestra,
                        principalTable: "tiposMuestras",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "valoresReferencia",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idNombreVariableMuestra = table.Column<int>(type: "int", nullable: true),
                    maximo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    minimo = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valoresReferencia", x => x.id);
                    table.ForeignKey(
                        name: "FK__valoresRe__idNom__7D439ABD",
                        column: x => x.idNombreVariableMuestra,
                        principalTable: "nombresVariablesMuestras",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "valoresVariablesMuestras",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idNombreVariableMuestra = table.Column<int>(type: "int", nullable: true),
                    idMuestra = table.Column<int>(type: "int", nullable: true),
                    valor = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valoresVariablesMuestras", x => x.id);
                    table.ForeignKey(
                        name: "FK__valoresVa__idMue__7B5B524B",
                        column: x => x.idMuestra,
                        principalTable: "muestras",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__valoresVa__idNom__7A672E12",
                        column: x => x.idNombreVariableMuestra,
                        principalTable: "nombresVariablesMuestras",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_muestras_idCampo",
                table: "muestras",
                column: "idCampo");

            migrationBuilder.CreateIndex(
                name: "IX_muestras_idEntidad",
                table: "muestras",
                column: "idEntidad");

            migrationBuilder.CreateIndex(
                name: "IX_muestras_idTipoMuestra",
                table: "muestras",
                column: "idTipoMuestra");

            migrationBuilder.CreateIndex(
                name: "IX_nombresVariablesMuestras_idTipoMuestra",
                table: "nombresVariablesMuestras",
                column: "idTipoMuestra");

            migrationBuilder.CreateIndex(
                name: "UQ__tiposMue__72AFBCC6620C8E8F",
                table: "tiposMuestras",
                column: "nombre",
                unique: true,
                filter: "[nombre] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_valoresDatosEstaticos_idEntidad",
                table: "valoresDatosEstaticos",
                column: "idEntidad");

            migrationBuilder.CreateIndex(
                name: "IX_valoresDatosEstaticos_idNombreDatoEstatico",
                table: "valoresDatosEstaticos",
                column: "idNombreDatoEstatico");

            migrationBuilder.CreateIndex(
                name: "IX_valoresReferencia_idNombreVariableMuestra",
                table: "valoresReferencia",
                column: "idNombreVariableMuestra",
                unique: true,
                filter: "[idNombreVariableMuestra] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_valoresVariablesMuestras_idMuestra",
                table: "valoresVariablesMuestras",
                column: "idMuestra");

            migrationBuilder.CreateIndex(
                name: "IX_valoresVariablesMuestras_idNombreVariableMuestra",
                table: "valoresVariablesMuestras",
                column: "idNombreVariableMuestra");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "valoresDatosEstaticos");

            migrationBuilder.DropTable(
                name: "valoresReferencia");

            migrationBuilder.DropTable(
                name: "valoresVariablesMuestras");

            migrationBuilder.DropTable(
                name: "nombresDatosEstaticos");

            migrationBuilder.DropTable(
                name: "muestras");

            migrationBuilder.DropTable(
                name: "nombresVariablesMuestras");

            migrationBuilder.DropTable(
                name: "campos");

            migrationBuilder.DropTable(
                name: "entidades");

            migrationBuilder.DropTable(
                name: "tiposMuestras");
        }
    }
}

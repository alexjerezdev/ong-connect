using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ONG_connect.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    IdProyecto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false),
                    Presupuesto = table.Column<decimal>(type: "numeric", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.IdProyecto);
                    table.ForeignKey(
                        name: "FK_Proyectos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Actividades",
                columns: table => new
                {
                    IdActividad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Responsable = table.Column<string>(type: "text", nullable: false),
                    IdProyecto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actividades", x => x.IdActividad);
                    table.ForeignKey(
                        name: "FK_Actividades_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beneficiarios",
                columns: table => new
                {
                    IdBeneficiario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    TipoBeneficiario = table.Column<string>(type: "text", nullable: false),
                    CantidadAyuda = table.Column<decimal>(type: "numeric", nullable: false),
                    IdProyecto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beneficiarios", x => x.IdBeneficiario);
                    table.ForeignKey(
                        name: "FK_Beneficiarios_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donaciones",
                columns: table => new
                {
                    IdDonacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreDonante = table.Column<string>(type: "text", nullable: false),
                    TipoDonacion = table.Column<string>(type: "text", nullable: false),
                    ValorEconomico = table.Column<decimal>(type: "numeric", nullable: false),
                    FechaDonacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdProyecto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donaciones", x => x.IdDonacion);
                    table.ForeignKey(
                        name: "FK_Donaciones_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    IdVoluntario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<bool>(type: "boolean", nullable: false),
                    IdProyecto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.IdVoluntario);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "IdProyecto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actividades_IdProyecto",
                table: "Actividades",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Beneficiarios_IdProyecto",
                table: "Beneficiarios",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Donaciones_IdProyecto",
                table: "Donaciones",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_IdUsuario",
                table: "Proyectos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_IdProyecto",
                table: "Voluntarios",
                column: "IdProyecto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actividades");

            migrationBuilder.DropTable(
                name: "Beneficiarios");

            migrationBuilder.DropTable(
                name: "Donaciones");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

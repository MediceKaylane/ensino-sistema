using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_ensino.Migrations
{
    /// <inheritdoc />
    public partial class CriarBancoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MateriaTurma",
                columns: table => new
                {
                    MateriasId = table.Column<int>(type: "int", nullable: false),
                    TurmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaTurma", x => new { x.MateriasId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_MateriaTurma_Materias_MateriasId",
                        column: x => x.MateriasId,
                        principalTable: "Materias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaTurma_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Matricula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimeiroAcessoConcluido = table.Column<bool>(type: "bit", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Aluno_TurmaId = table.Column<int>(type: "int", nullable: true),
                    TurmaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Matricula);
                    table.ForeignKey(
                        name: "FK_Usuario_Turmas_Aluno_TurmaId",
                        column: x => x.Aluno_TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuario_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArquivoOuLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrutorMatricula = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MateriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atividades_Materias_MateriaId",
                        column: x => x.MateriaId,
                        principalTable: "Materias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Atividades_Usuario_InstrutorMatricula",
                        column: x => x.InstrutorMatricula,
                        principalTable: "Usuario",
                        principalColumn: "Matricula");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_InstrutorMatricula",
                table: "Atividades",
                column: "InstrutorMatricula");

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_MateriaId",
                table: "Atividades",
                column: "MateriaId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaTurma_TurmaId",
                table: "MateriaTurma",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Aluno_TurmaId",
                table: "Usuario",
                column: "Aluno_TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_TurmaId",
                table: "Usuario",
                column: "TurmaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atividades");

            migrationBuilder.DropTable(
                name: "MateriaTurma");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Materias");

            migrationBuilder.DropTable(
                name: "Turmas");
        }
    }
}

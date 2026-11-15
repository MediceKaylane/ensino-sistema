using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_ensino.Migrations
{
    public partial class VincularPublicacoesATurmas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>("TurmaId", "Atividades", nullable: true);

            migrationBuilder.CreateTable(
                name: "InstrutorTurma",
                columns: table => new
                {
                    InstrutoresMatricula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TurmasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrutorTurma", x => new { x.InstrutoresMatricula, x.TurmasId });
                    table.ForeignKey("FK_InstrutorTurma_Usuario_InstrutoresMatricula",
                        x => x.InstrutoresMatricula, "Usuario", "Matricula", onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_InstrutorTurma_Turmas_TurmasId",
                        x => x.TurmasId, "Turmas", "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO dbo.InstrutorTurma (InstrutoresMatricula, TurmasId)
                SELECT Matricula, TurmaId
                FROM dbo.Usuario
                WHERE Discriminator = N'Instrutor' AND TurmaId IS NOT NULL;

                UPDATE atividade
                SET TurmaId = vinculo.TurmasId
                FROM dbo.Atividades AS atividade
                INNER JOIN dbo.InstrutorTurma AS vinculo
                    ON vinculo.InstrutoresMatricula = atividade.InstrutorMatricula
                INNER JOIN dbo.MateriaTurma AS materia
                    ON materia.TurmaId = vinculo.TurmasId
                    AND materia.MateriasId = atividade.MateriaId;
                """);

            migrationBuilder.CreateIndex("IX_InstrutorTurma_TurmasId", "InstrutorTurma", "TurmasId");
            migrationBuilder.CreateIndex("IX_Atividades_TurmaId", "Atividades", "TurmaId");
            migrationBuilder.AddForeignKey("FK_Atividades_Turmas_TurmaId", "Atividades",
                "TurmaId", "Turmas", principalColumn: "Id");

            migrationBuilder.DropForeignKey("FK_Usuario_Turmas_TurmaId", "Usuario");
            migrationBuilder.DropIndex("IX_Usuario_TurmaId", "Usuario");
            migrationBuilder.DropColumn("TurmaId", "Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT InstrutoresMatricula FROM dbo.InstrutorTurma
                    GROUP BY InstrutoresMatricula HAVING COUNT(*) > 1
                ) OR EXISTS (
                    SELECT 1 FROM dbo.Atividades AS atividade
                    WHERE atividade.TurmaId IS NOT NULL
                      AND NOT EXISTS (
                          SELECT 1 FROM dbo.InstrutorTurma AS vinculo
                          WHERE vinculo.InstrutoresMatricula = atividade.InstrutorMatricula
                            AND vinculo.TurmasId = atividade.TurmaId
                      )
                )
                    THROW 50001, N'O modelo anterior não comporta os vínculos atuais de turmas.', 1;
                """);

            migrationBuilder.AddColumn<int>("TurmaId", "Usuario", nullable: true);
            migrationBuilder.Sql("""
                UPDATE usuario SET TurmaId = vinculo.TurmasId
                FROM dbo.Usuario AS usuario
                INNER JOIN dbo.InstrutorTurma AS vinculo
                    ON vinculo.InstrutoresMatricula = usuario.Matricula;
                """);
            migrationBuilder.CreateIndex("IX_Usuario_TurmaId", "Usuario", "TurmaId");
            migrationBuilder.AddForeignKey("FK_Usuario_Turmas_TurmaId", "Usuario",
                "TurmaId", "Turmas", principalColumn: "Id");

            migrationBuilder.DropForeignKey("FK_Atividades_Turmas_TurmaId", "Atividades");
            migrationBuilder.DropIndex("IX_Atividades_TurmaId", "Atividades");
            migrationBuilder.DropColumn("TurmaId", "Atividades");
            migrationBuilder.DropTable("InstrutorTurma");
        }
    }
}

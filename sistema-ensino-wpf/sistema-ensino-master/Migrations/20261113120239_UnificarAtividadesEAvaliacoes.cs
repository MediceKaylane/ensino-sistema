using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistema_ensino.Migrations
{
    /// <inheritdoc />
    public partial class UnificarAtividadesEAvaliacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoriaAtividadeId",
                table: "Atividades",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nota = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AlunoMatricula = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AtividadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Atividades_AtividadeId",
                        column: x => x.AtividadeId,
                        principalTable: "Atividades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Usuario_AlunoMatricula",
                        column: x => x.AlunoMatricula,
                        principalTable: "Usuario",
                        principalColumn: "Matricula");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_AlunoMatricula",
                table: "Avaliacoes",
                column: "AlunoMatricula");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_AtividadeId",
                table: "Avaliacoes",
                column: "AtividadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaAtividadeId",
                table: "Atividades",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

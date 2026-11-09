using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_ensino.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTiposConcretosAtividade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoAtividade",
                table: "Atividades",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE Atividades
                SET TipoAtividade = CASE CategoriaAtividadeId
                    WHEN 1 THEN 'Prova'
                    WHEN 2 THEN 'Trabalho'
                    WHEN 3 THEN 'Exercicio'
                    WHEN 4 THEN 'Seminario'
                    WHEN 5 THEN 'Projeto'
                    ELSE 'Projeto'
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoAtividade",
                table: "Atividades");
        }
    }
}

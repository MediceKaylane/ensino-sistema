using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_ensino.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaCategoriaAtividade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasAtividade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasAtividade", x => x.Id);
                });

            migrationBuilder.Sql(
                "SET IDENTITY_INSERT CategoriasAtividade ON; " +
                "INSERT INTO CategoriasAtividade (Id, Nome) VALUES " +
                "(1, 'Prova'), (2, 'Trabalho'), (3, 'Exercicio'), " +
                "(4, 'Seminario'), (5, 'Projeto'); " +
                "SET IDENTITY_INSERT CategoriasAtividade OFF;");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaAtividadeId",
                table: "Atividades",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE Atividades SET CategoriaAtividadeId = CASE Categoria " +
                "WHEN 0 THEN 1 WHEN 1 THEN 2 WHEN 2 THEN 3 WHEN 3 THEN 4 WHEN 4 THEN 5 ELSE 1 END");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaAtividadeId",
                table: "Atividades",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Atividades");

            migrationBuilder.CreateIndex(
                name: "IX_Atividades_CategoriaAtividadeId",
                table: "Atividades",
                column: "CategoriaAtividadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Atividades_CategoriasAtividade_CategoriaAtividadeId",
                table: "Atividades",
                column: "CategoriaAtividadeId",
                principalTable: "CategoriasAtividade",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atividades_CategoriasAtividade_CategoriaAtividadeId",
                table: "Atividades");

            migrationBuilder.DropIndex(
                name: "IX_Atividades_CategoriaAtividadeId",
                table: "Atividades");

            migrationBuilder.AddColumn<int>(
                name: "Categoria",
                table: "Atividades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE Atividades SET Categoria = CASE CategoriaAtividadeId " +
                "WHEN 1 THEN 0 WHEN 2 THEN 1 WHEN 3 THEN 2 WHEN 4 THEN 3 WHEN 5 THEN 4 ELSE 0 END");

            migrationBuilder.DropColumn(
                name: "CategoriaAtividadeId",
                table: "Atividades");

            migrationBuilder.DropTable(
                name: "CategoriasAtividade");
        }
    }
}

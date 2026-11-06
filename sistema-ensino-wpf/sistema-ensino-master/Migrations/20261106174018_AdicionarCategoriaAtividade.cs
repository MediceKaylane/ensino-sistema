using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistema_ensino.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCategoriaAtividade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Categoria",
                table: "Atividades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Atividades");
        }
    }
}


using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;

namespace sistema_ensino.Repository
{
    public class AtividadeRepository
    {
        private readonly AppDbContext _contexto;

        public AtividadeRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma atividade pelo ID.
        public AtividadeBase? BuscarAtividadePorId(int id)
        {
            return _contexto.Atividades
                .FirstOrDefault(atividade => atividade.Id == id);
        }

        // Consulta todas as atividades cadastradas.
        public List<AtividadeBase> ConsultarAtividades()
        {
            return _contexto.Atividades.ToList();
        }

        // Consulta atividades por categoria utilizando SQL direto.
        public List<AtividadeBase> ConsultarAtividadesPorCategoria(int categoriaId)
        {
            return _contexto.Atividades
                .FromSqlInterpolated(
                    $"SELECT * FROM Atividades WHERE CategoriaAtividadeId = {categoriaId}"
                )
                .Include(atividade => atividade.Categoria)
                .ToList();
        }


        // Consulta atividades por categoria utilizando ADO.NET, sem Entity Framework.
        public List<AtividadeBase> ConsultarAtividadesPorCategoriaSemEF(int categoriaId)
        {
            var atividades = new List<AtividadeBase>();

            string connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SistemaEnsinoDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            string sql = @"
        SELECT
            a.Id,
            a.Titulo,
            a.Descricao,
            a.ArquivoOuLink,
            a.CategoriaAtividadeId,
            c.Id AS CategoriaId,
            c.Nome AS CategoriaNome
        FROM Atividades AS a
        INNER JOIN CategoriasAtividade AS c
            ON c.Id = a.CategoriaAtividadeId
        WHERE a.CategoriaAtividadeId = @categoriaId;";

            using (var conexao = new SqlConnection(connectionString))
            {
                conexao.Open();

                using (var comando = new SqlCommand(sql, conexao))
                {
                    comando.Parameters.Add("@categoriaId", SqlDbType.Int).Value = categoriaId;

                    using (var leitor = comando.ExecuteReader())
                    {
                        while (leitor.Read())
                        {
                            var categoria = new CategoriaAtividade(
                                leitor.GetInt32(leitor.GetOrdinal("CategoriaId")),
                                leitor.GetString(leitor.GetOrdinal("CategoriaNome"))
                            );

                            var atividade = CriarAtividade(
                                leitor.GetInt32(leitor.GetOrdinal("Id")),
                                leitor.GetString(leitor.GetOrdinal("Titulo")),
                                leitor.GetString(leitor.GetOrdinal("Descricao")),
                                leitor.GetString(leitor.GetOrdinal("ArquivoOuLink")),
                                leitor.GetInt32(leitor.GetOrdinal("CategoriaAtividadeId")),
                                categoria
                            );

                            atividades.Add(atividade);
                        }
                    }
                }
            }

            return atividades;
        }

        private static AtividadeBase CriarAtividade(
            int id,
            string titulo,
            string descricao,
            string arquivoOuLink,
            int categoriaAtividadeId,
            CategoriaAtividade categoria)
        {
            return categoriaAtividadeId switch
            {
                1 => new Prova(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria),
                2 => new Trabalho(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria),
                3 => new Exercicio(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria),
                4 => new Seminario(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria),
                5 => new Projeto(id, titulo, descricao, arquivoOuLink, categoriaAtividadeId, categoria),
                _ => throw new InvalidOperationException("Categoria de atividade desconhecida.")
            };
        }

    }
}

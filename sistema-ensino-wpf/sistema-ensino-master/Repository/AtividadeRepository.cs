using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class AtividadeRepository
    {
        private readonly AppDbContext _contexto;

        public AtividadeRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        public Atividades? BuscarAtividadePorId(int id)
        {
            return _contexto.Atividades
                .Include(atividade => atividade.Instrutor)
                .Include(atividade => atividade.Materia)
                .FirstOrDefault(atividade => atividade.Id == id);
        }

        public List<Atividades> ConsultarAtividades()
        {
            return _contexto.Atividades
                .Include(atividade => atividade.Instrutor)
                .Include(atividade => atividade.Materia)
                .ToList();
        }

        public List<Atividades> ConsultarAtividadesPorMateria(int materiaId)
        {
            return _contexto.Atividades
                .FromSqlInterpolated(
                    $"SELECT * FROM Atividades WHERE MateriaId = {materiaId}")
                .Include(atividade => atividade.Instrutor)
                .Include(atividade => atividade.Materia)
                .ToList();
        }

        public List<Atividades> ConsultarAtividadesPorMateriaSemEF(int materiaId)
        {
            var atividades = new List<Atividades>();
            string connectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SistemaEnsinoDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";

            const string sql = @"
                SELECT a.Id, a.Titulo, a.Descricao, a.ArquivoOuLink,
                       u.Matricula AS InstrutorMatricula,
                       m.Id AS MateriaId, m.Nome AS MateriaNome, m.Tipo AS MateriaTipo
                FROM Atividades AS a
                INNER JOIN Materias AS m ON m.Id = a.MateriaId
                LEFT JOIN Usuario AS u
                    ON u.Matricula = a.InstrutorMatricula AND u.Discriminator = 'Instrutor'
                WHERE a.MateriaId = @materiaId;";

            using var conexao = new SqlConnection(connectionString);
            conexao.Open();

            using var comando = new SqlCommand(sql, conexao);
            comando.Parameters.Add("@materiaId", SqlDbType.Int).Value = materiaId;

            using var leitor = comando.ExecuteReader();
            while (leitor.Read())
            {
                Instrutor? instrutor = leitor.IsDBNull(leitor.GetOrdinal("InstrutorMatricula"))
                    ? null
                    : new Instrutor(leitor.GetString(leitor.GetOrdinal("InstrutorMatricula")));
                var materia = new Materia(
                    leitor.GetInt32(leitor.GetOrdinal("MateriaId")),
                    leitor.GetString(leitor.GetOrdinal("MateriaNome")),
                    leitor.GetString(leitor.GetOrdinal("MateriaTipo")));

                atividades.Add(new Atividades(
                    leitor.GetInt32(leitor.GetOrdinal("Id")),
                    leitor.GetString(leitor.GetOrdinal("Titulo")),
                    leitor.GetString(leitor.GetOrdinal("Descricao")),
                    leitor.GetString(leitor.GetOrdinal("ArquivoOuLink")),
                    instrutor,
                    materia));
            }

            return atividades;
        }
    }
}

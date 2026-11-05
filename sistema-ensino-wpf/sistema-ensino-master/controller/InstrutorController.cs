
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas aos instrutores.
    public class InstrutorController
    {
        private readonly AppDbContext _contexto;

        // DIP: recebe a dependência pelo construtor.
        public InstrutorController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca um instrutor pela matrícula no banco de dados.
        public Instrutor? BuscarInstrutorPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _contexto.Instrutores
                .Include(instrutor => instrutor.Atividades)
                .FirstOrDefault(instrutor => instrutor.Matricula == matricula);
        }

        // Publica uma atividade e salva no banco de dados.
        public void PublicarAtividade(string matricula, Atividade atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            Instrutor? instrutor = BuscarInstrutorPorMatricula(matricula);

            if (instrutor is null)
            {
                throw new InvalidOperationException("Instrutor não encontrado.");
            }

            instrutor.PublicarAtividade(atividade);

            _contexto.SaveChanges();
        }

        // Consulta as atividades cadastradas pelo instrutor.
        public List<Atividade> ConsultarAtividades(string matricula)
        {
            Instrutor? instrutor = BuscarInstrutorPorMatricula(matricula);

            if (instrutor is null)
            {
                return new List<Atividade>();
            }

            return instrutor.Atividades;
        }
    }
}

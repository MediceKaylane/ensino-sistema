
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    public class AlunoController
    {
        private readonly AppDbContext _contexto;

        public AlunoController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca um aluno pela matrícula no banco de dados.
        public Aluno? BuscarAlunoPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            return _contexto.Alunos
                .Include(aluno => aluno.Turma)
                    .ThenInclude(turma => turma!.Instrutores)
                        .ThenInclude(instrutor => instrutor.Atividades)
                .FirstOrDefault(aluno => aluno.Matricula == matricula);
        }

        // Consulta as atividades disponibilizadas pelos instrutores da turma.
        public List<Atividade> VisualizarAtividades(string matricula)
        {
            Aluno? aluno = BuscarAlunoPorMatricula(matricula);

            if (aluno is null)
            {
                return new List<Atividade>();
            }

            return aluno.VisualizarAtividades();
        }

        // Verifica se o aluno existe antes de enviar uma resposta.
        public string EnviarResposta(string matricula)
        {
            Aluno? aluno = BuscarAlunoPorMatricula(matricula);

            if (aluno is null)
            {
                return "Aluno não encontrado.";
            }

            return aluno.EnviarResposta();
        }
    }
}

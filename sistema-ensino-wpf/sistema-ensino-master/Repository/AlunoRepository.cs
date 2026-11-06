using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class AlunoRepository
    {
        private readonly AppDbContext _contexto;

        public AlunoRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca um aluno pela matrícula, incluindo seus relacionamentos.
        public Aluno? BuscarAlunoPorMatricula(string matricula)
        {
            return _contexto.Alunos
                .Include(aluno => aluno.Turma)
                    .ThenInclude(turma => turma!.Instrutores)
                        .ThenInclude(instrutor => instrutor.Atividades)
                .FirstOrDefault(aluno => aluno.Matricula == matricula);
        }
    }
}
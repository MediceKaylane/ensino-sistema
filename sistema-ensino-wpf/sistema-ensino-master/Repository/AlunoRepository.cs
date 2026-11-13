using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public Aluno? BuscarAlunoPorMatricula(string matricula)
        {
            return _contexto.Alunos
                .Include(aluno => aluno.Turma)
                    .ThenInclude(turma => turma!.Materias)
                .Include(aluno => aluno.Turma)
                    .ThenInclude(turma => turma!.Instrutores)
                        .ThenInclude(instrutor => instrutor.Atividades)
                            .ThenInclude(atividade => atividade.Materia)
                .FirstOrDefault(aluno => aluno.Matricula == matricula);
        }

        public List<Avaliacoes> BuscarAvaliacoesDoAluno(Aluno alunoAutenticado)
        {
            if (alunoAutenticado is null)
            {
                throw new ArgumentNullException(nameof(alunoAutenticado));
            }

            return _contexto.Avaliacoes
                .Where(avaliacao => avaliacao.AlunoMatricula == alunoAutenticado.Matricula)
                .Include(avaliacao => avaliacao.Atividade)
                    .ThenInclude(atividade => atividade.Materia)
                .ToList();
        }
    }
}

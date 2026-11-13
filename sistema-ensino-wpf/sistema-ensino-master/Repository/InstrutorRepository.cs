using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class InstrutorRepository
    {
        private readonly AppDbContext _contexto;

        public InstrutorRepository(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        public Instrutor? BuscarInstrutorPorMatricula(string matricula)
        {
            return _contexto.Instrutores
                .Include(instrutor => instrutor.Turma)
                    .ThenInclude(turma => turma!.Materias)
                .Include(instrutor => instrutor.Turma)
                    .ThenInclude(turma => turma!.Alunos)
                .Include(instrutor => instrutor.Atividades)
                    .ThenInclude(atividade => atividade.Materia)
                .FirstOrDefault(instrutor => instrutor.Matricula == matricula);
        }

        public void PublicarAtividade(string matricula, Atividades atividade, int materiaId)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            Instrutor instrutor = BuscarInstrutorPorMatricula(matricula)
                ?? throw new InvalidOperationException("Instrutor não encontrado.");
            Materia materia = instrutor.Turma?.Materias.FirstOrDefault(item => item.Id == materiaId)
                ?? throw new InvalidOperationException("A matéria não pertence à turma do instrutor.");

            atividade.VincularInstrutor(instrutor);
            atividade.VincularMateria(materia);
            instrutor.PublicarAtividade(atividade);
            _contexto.Atividades.Add(atividade);
            _contexto.SaveChanges();
        }

        public void PublicarAvaliacao(
            string instrutorMatricula,
            string alunoMatricula,
            int atividadeId,
            decimal nota)
        {
            Instrutor instrutor = BuscarInstrutorPorMatricula(instrutorMatricula)
                ?? throw new InvalidOperationException("Instrutor não encontrado.");
            Aluno aluno = _contexto.Alunos
                .Include(item => item.Turma)
                .FirstOrDefault(item => item.Matricula == alunoMatricula)
                ?? throw new InvalidOperationException("Aluno não encontrado.");
            Atividades atividade = _contexto.Atividades
                .Include(item => item.Instrutor)
                .Include(item => item.Materia)
                .FirstOrDefault(item => item.Id == atividadeId)
                ?? throw new InvalidOperationException("Atividade não encontrada.");

            if (instrutor.Turma is null || aluno.Turma?.Id != instrutor.Turma.Id)
            {
                throw new InvalidOperationException("O aluno não pertence à turma do instrutor.");
            }

            if (atividade.InstrutorMatricula != instrutor.Matricula ||
                !instrutor.Turma.Materias.Any(materia => materia.Id == atividade.MateriaId))
            {
                throw new InvalidOperationException("A atividade não pertence ao instrutor e à sua turma.");
            }

            var avaliacao = new Avaliacoes(nota, aluno, atividade);
            avaliacao.Publicar();
            _contexto.Avaliacoes.Add(avaliacao);
            _contexto.SaveChanges();
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}

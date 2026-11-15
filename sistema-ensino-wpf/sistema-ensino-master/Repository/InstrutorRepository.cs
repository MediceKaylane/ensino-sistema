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
                .AsSplitQuery()
                .Include(instrutor => instrutor.Turmas)
                    .ThenInclude(turma => turma.Materias)
                .Include(instrutor => instrutor.Turmas)
                    .ThenInclude(turma => turma.Alunos)
                .Include(instrutor => instrutor.Atividades)
                    .ThenInclude(atividade => atividade.Materia)
                .FirstOrDefault(instrutor => instrutor.Matricula == matricula);
        }

        public void PublicarAtividade(string matricula, Atividades atividade, int turmaId, int materiaId)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            Instrutor instrutor = BuscarInstrutorPorMatricula(matricula)
                ?? throw new InvalidOperationException("Instrutor não encontrado.");
            Turma turma = instrutor.Turmas.FirstOrDefault(item => item.Id == turmaId)
                ?? throw new InvalidOperationException("Selecione uma turma vinculada ao instrutor.");
            Materia materia = turma.Materias.FirstOrDefault(item => item.Id == materiaId)
                ?? throw new InvalidOperationException("A matéria não pertence à turma do instrutor.");

            atividade.VincularInstrutor(instrutor);
            atividade.VincularMateria(materia);
            atividade.VincularTurma(turma);
            instrutor.PublicarAtividade(atividade);
            _contexto.Atividades.Add(atividade);
            _contexto.SaveChanges();
        }

        public void PublicarAvaliacao(
            string instrutorMatricula,
            int turmaId,
            string alunoMatricula,
            int atividadeId,
            decimal nota)
        {
            Instrutor instrutor = BuscarInstrutorPorMatricula(instrutorMatricula)
                ?? throw new InvalidOperationException("Instrutor não encontrado.");
            Turma turma = instrutor.Turmas.FirstOrDefault(item => item.Id == turmaId)
                ?? throw new InvalidOperationException("Selecione uma turma vinculada ao instrutor.");
            Aluno aluno = turma.Alunos.FirstOrDefault(item => item.Matricula == alunoMatricula)
                ?? throw new InvalidOperationException("O aluno não pertence à turma selecionada.");
            Atividades atividade = instrutor.Atividades.FirstOrDefault(item => item.Id == atividadeId &&
                item.TurmaId == turmaId && turma.Materias.Any(materia => materia.Id == item.MateriaId))
                ?? throw new InvalidOperationException("A atividade não pertence ao instrutor e à turma selecionada.");

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

using System;
using System.Linq;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às turmas.
    public class TurmaController
    {
        private readonly TurmaRepository _turmaRepository;

        // DIP: recebe o repositório pelo construtor.
        public TurmaController(TurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository
                ?? throw new ArgumentNullException(nameof(turmaRepository));
        }

        // Busca uma turma e carrega seus relacionamentos.
        public Turma? BuscarTurmaPorId(int id)
        {
            return _turmaRepository.BuscarTurmaPorId(id);
        }

        // Associa uma matéria à turma.
        public void AdicionarMateria(int idTurma, Materia materia)
        {
            if (materia is null)
                throw new ArgumentNullException(nameof(materia));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Materia? materiaBanco =
                _turmaRepository.BuscarMateriaPorId(materia.Id);

            if (materiaBanco is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            turma.AdicionarMateria(materiaBanco);

            _turmaRepository.Salvar();
        }

        // Remove uma matéria da turma.
        public void RemoverMateria(int idTurma, Materia materia)
        {
            if (materia is null)
                throw new ArgumentNullException(nameof(materia));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Materia? materiaAssociada = turma.Materias
                .FirstOrDefault(m => m.Id == materia.Id);

            if (materiaAssociada is null)
                throw new InvalidOperationException("Matéria não associada à turma.");

            turma.RemoverMateria(materiaAssociada);

            _turmaRepository.Salvar();
        }

        // Associa um instrutor à turma.
        public void AdicionarInstrutor(int idTurma, Instrutor instrutor)
        {
            if (instrutor is null)
                throw new ArgumentNullException(nameof(instrutor));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Instrutor? instrutorBanco =
                _turmaRepository.BuscarInstrutorPorMatricula(instrutor.Matricula);

            if (instrutorBanco is null)
                throw new InvalidOperationException("Instrutor não encontrado.");

            turma.AdicionarInstrutor(instrutorBanco);

            _turmaRepository.Salvar();
        }

        // Associa um aluno à turma.
        public void AdicionarAluno(int idTurma, Aluno aluno)
        {
            if (aluno is null)
                throw new ArgumentNullException(nameof(aluno));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Aluno? alunoBanco =
                _turmaRepository.BuscarAlunoPorMatricula(aluno.Matricula);

            if (alunoBanco is null)
                throw new InvalidOperationException("Aluno não encontrado.");

            turma.AdicionarAluno(alunoBanco);

            _turmaRepository.Salvar();
        }
    }
}
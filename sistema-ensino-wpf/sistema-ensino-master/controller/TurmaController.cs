
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sistema_ensino.model;
using sistema_ensino.data;

namespace sistema_ensino.controller
{
    // SRP: coordena as operações relacionadas às turmas.
    public class TurmaController
    {
        // DIP: recebe o contexto do banco pelo construtor.
        private readonly AppDbContext _contexto;

        public TurmaController(AppDbContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        // Busca uma turma e carrega seus relacionamentos.
        public Turma? BuscarTurmaPorId(int id)
        {
            return _contexto.Turmas
                .Include(turma => turma.Materias)
                .Include(turma => turma.Alunos)
                .Include(turma => turma.Instrutores)
                .FirstOrDefault(turma => turma.Id == id);
        }

        // Associa uma matéria à turma.
        public void AdicionarMateria(int idTurma, Materia materia)
        {
            if (materia is null)
                throw new ArgumentNullException(nameof(materia));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Materia? materiaBanco = _contexto.Materias.Find(materia.Id);

            if (materiaBanco is null)
                throw new InvalidOperationException("Matéria não encontrada.");

            turma.AdicionarMateria(materiaBanco);

            _contexto.SaveChanges();
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

            _contexto.SaveChanges();
        }

        // Associa um instrutor à turma.
        public void AdicionarInstrutor(int idTurma, Instrutor instrutor)
        {
            if (instrutor is null)
                throw new ArgumentNullException(nameof(instrutor));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Instrutor? instrutorBanco = _contexto.Instrutores
                .Find(instrutor.Matricula);

            if (instrutorBanco is null)
                throw new InvalidOperationException("Instrutor não encontrado.");

            turma.AdicionarInstrutor(instrutorBanco);

            _contexto.SaveChanges();
        }

        // Associa um aluno à turma.
        public void AdicionarAluno(int idTurma, Aluno aluno)
        {
            if (aluno is null)
                throw new ArgumentNullException(nameof(aluno));

            Turma? turma = BuscarTurmaPorId(idTurma);

            if (turma is null)
                throw new InvalidOperationException("Turma não encontrada.");

            Aluno? alunoBanco = _contexto.Alunos.Find(aluno.Matricula);

            if (alunoBanco is null)
                throw new InvalidOperationException("Aluno não encontrado.");

            turma.AdicionarAluno(alunoBanco);

            _contexto.SaveChanges();
        }
    }
}

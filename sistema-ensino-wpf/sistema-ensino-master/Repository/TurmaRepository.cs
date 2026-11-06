using sistema_ensino.data;
using sistema_ensino.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace sistema_ensino.Repository
{
    public class TurmaRepository
    {
        private readonly AppDbContext _contexto;

        public TurmaRepository(AppDbContext contexto)
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

        // Busca uma matéria pelo ID.
        public Materia? BuscarMateriaPorId(int id)
        {
            return _contexto.Materias.Find(id);
        }

        // Busca um instrutor pela matrícula.
        public Instrutor? BuscarInstrutorPorMatricula(string matricula)
        {
            return _contexto.Instrutores.Find(matricula);
        }

        // Busca um aluno pela matrícula.
        public Aluno? BuscarAlunoPorMatricula(string matricula)
        {
            return _contexto.Alunos.Find(matricula);
        }

        // Salva as alterações no banco.
        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
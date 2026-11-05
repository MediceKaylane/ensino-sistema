using System.Collections.Generic;
using System;

namespace sistema_ensino.model
{
    public class Turma
    {
        public int Id { get; private set; }
        public string Tipo { get; private set; }
        public List<Instrutor> Instrutores { get; private set; } = new();
        public List<Materia> Materias { get; private set; } = new();
        public List<Aluno> Alunos { get; private set; } = new();

        public Turma(int id, string tipo)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O identificador da turma deve ser positivo.");
            }

            if (string.IsNullOrWhiteSpace(tipo))
            {
                throw new ArgumentException("O tipo da turma é obrigatório.");
            }

            Id = id;
            Tipo = tipo;
        }


        public void AdicionarMateria(Materia materia)
        {
            if (materia is null)
            {
                throw new ArgumentNullException(nameof(materia));
            }

            Materias.Add(materia);
        }

        public void RemoverMateria(Materia materia)
        {
            if (materia is null)
            {
                throw new ArgumentNullException(nameof(materia));
            }

            Materias.Remove(materia);
        }

        public void AdicionarInstrutor(Instrutor instrutor)
        {
            if (instrutor is null)
            {
                throw new ArgumentNullException(nameof(instrutor));
            }

            Instrutores.Add(instrutor);
            instrutor.VincularTurma(this);
        }

        public void AdicionarAluno(Aluno aluno)
        {
            if (aluno is null)
            {
                throw new ArgumentNullException(nameof(aluno));
            }

            Alunos.Add(aluno);
            aluno.VincularTurma(this);
        }
    }
}
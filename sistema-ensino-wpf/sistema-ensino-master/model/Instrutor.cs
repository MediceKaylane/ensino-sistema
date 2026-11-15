using sistema_ensino.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_ensino.model
{
    public class Instrutor : Usuario
    {
        public List<Turma> Turmas { get; private set; } = new();
        public List<Atividades> Atividades { get; private set; } = new();
        public Instrutor(string matricula) : base(matricula)
        {
        }
        public void VincularTurma(Turma turma)
        {
            if (turma is null)
            {
                throw new ArgumentNullException(nameof(turma));
            }

            if (!Turmas.Contains(turma))
            {
                Turmas.Add(turma);
            }

            if (!turma.Instrutores.Contains(this))
            {
                turma.Instrutores.Add(this);
            }
        }
        public void PublicarAtividade(Atividades atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            atividade.Publicar();

            if (atividade.InstrutorMatricula != Matricula)
            {
                throw new InvalidOperationException("A atividade pertence a outro instrutor.");
            }

            if (!Turmas.Any(turma => turma.Id == atividade.TurmaId &&
                turma.Materias.Any(materia => materia.Id == atividade.MateriaId)))
            {
                throw new InvalidOperationException("A matéria não pertence à turma do instrutor.");
            }

            if (!Atividades.Contains(atividade))
            {
                Atividades.Add(atividade);
            }
        }
    }
}
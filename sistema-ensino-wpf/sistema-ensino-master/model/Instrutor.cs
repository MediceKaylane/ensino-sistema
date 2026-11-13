using sistema_ensino.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_ensino.model
{
    public class Instrutor : Usuario
    {
        public Turma? Turma { get; private set; }
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

            Turma = turma;
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

            if (Turma is null || !Turma.Materias.Any(materia => materia.Id == atividade.MateriaId))
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
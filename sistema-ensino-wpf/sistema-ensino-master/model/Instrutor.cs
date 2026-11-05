using sistema_ensino.model;
using System;
using System.Collections.Generic;

namespace sistema_ensino.model
{
    public class Instrutor : Usuario
    {
        public Turma? Turma { get; private set; }
        public List<Atividade> Atividades { get; private set; } = new();
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
        public void PublicarAtividade(Atividade atividade)
        {
            if (atividade is null)
            {
                throw new ArgumentNullException(nameof(atividade));
            }

            Atividades.Add(atividade);
        }
    }
}
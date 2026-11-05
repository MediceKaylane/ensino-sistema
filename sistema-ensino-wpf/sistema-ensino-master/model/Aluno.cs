using sistema_ensino.model;
using System;
using System.Collections.Generic;

namespace sistema_ensino.model
{
    public class Aluno : Usuario
    {
        public Turma? Turma { get; private set; }
        public Aluno(string matricula) : base(matricula)
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

        public List<Atividade> VisualizarAtividades()
        {
            if (Turma is null)
            {
                return new List<Atividade>();
            }

            var atividades = new List<Atividade>();

            foreach (var instrutor in Turma.Instrutores)
            {
                atividades.AddRange(instrutor.Atividades);
            }

            return atividades;
        }

        public string EnviarResposta()
        {
            return "Resposta enviada com sucesso!";
        }

       

    }
}
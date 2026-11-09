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


            //bd

        }

        public List<AtividadeBase> VisualizarAtividades()
        {
            if (Turma is null)
            {
                return new List<AtividadeBase>();
            }

            var atividades = new List<AtividadeBase>();

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
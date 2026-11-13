using sistema_ensino.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_ensino.model
{
    public class Aluno : Usuario
    {
        public Turma? Turma { get; private set; }
        public List<Avaliacoes> Avaliacoes { get; private set; } = new();

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

        public List<Atividades> VisualizarAtividades()
        {
            if (Turma is null)
            {
                return new List<Atividades>();
            }

            return Turma.Instrutores
                .Where(instrutor => instrutor.Turma?.Id == Turma.Id)
                .SelectMany(instrutor => instrutor.Atividades)
                .Where(atividade =>
                    atividade.InstrutorMatricula == atividade.Instrutor?.Matricula &&
                    atividade.MateriaId.HasValue &&
                    Turma.Materias.Any(materia => materia.Id == atividade.MateriaId.Value))
                .DistinctBy(atividade => atividade.Id)
                .ToList();
        }

        public List<Avaliacoes> VisualizarAvaliacoes()
        {
            return Avaliacoes
                .Where(avaliacao => avaliacao.AlunoMatricula == Matricula)
                .ToList();
        }

        public string EnviarResposta()
        {
            return "Resposta enviada com sucesso!";
        }

       

    }
}
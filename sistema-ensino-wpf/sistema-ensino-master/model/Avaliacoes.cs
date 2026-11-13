using System;

namespace sistema_ensino.model
{
    public class Avaliacoes : IPublicar
    {
        public int Id { get; private set; }
        public decimal Nota { get; private set; }
        public string AlunoMatricula { get; private set; } = string.Empty;
        public Aluno Aluno { get; private set; } = null!;
        public int AtividadeId { get; private set; }
        public Atividades Atividade { get; private set; } = null!;

        private Avaliacoes()
        {
        }

        public Avaliacoes(decimal nota, Aluno aluno, Atividades atividade)
        {
            Nota = nota;
            Aluno = aluno ?? throw new ArgumentNullException(nameof(aluno));
            Atividade = atividade ?? throw new ArgumentNullException(nameof(atividade));
            AlunoMatricula = aluno.Matricula;
            AtividadeId = atividade.Id;
        }

        public void Publicar()
        {
            if (Aluno is null || string.IsNullOrWhiteSpace(AlunoMatricula))
            {
                throw new InvalidOperationException("A avaliação deve estar vinculada a um aluno.");
            }

            if (Atividade is null || AtividadeId <= 0)
            {
                throw new InvalidOperationException("A avaliação deve estar vinculada a uma atividade salva.");
            }
        }
    }
}

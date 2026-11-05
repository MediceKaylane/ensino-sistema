using System.Collections.Generic;
using System;

namespace sistema_ensino.model
{
    public class CadastroSimulado
    {
        public List<Aluno> Alunos { get; private set; } = new();
        public List<Instrutor> Instrutores { get; private set; } = new();

        public void CarregarMatriculasPreCadastradas()
        {
            if (Alunos.Count > 0 || Instrutores.Count > 0)
            {
                return;
            }

            Alunos.Add(new Aluno("A001"));
            Alunos.Add(new Aluno("A002"));
            Instrutores.Add(new Instrutor("I001"));
        }

        public Usuario? BuscarPorMatricula(string matricula)
        {
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return null;
            }

            foreach (var aluno in Alunos)
            {
                if (aluno.Matricula == matricula)
                {
                    return aluno;
                }
            }

            foreach (var instrutor in Instrutores)
            {
                if (instrutor.Matricula == matricula)
                {
                    return instrutor;
                }
            }

            return null;
        }

        public bool VerificarPrimeiroAcesso(string matricula)
        {
            Usuario? usuario = BuscarPorMatricula(matricula);

            return usuario is not null && !usuario.PrimeiroAcessoConcluido;
        }

        public bool SalvarPrimeiroAcesso(string matricula, string nome, string email)
        {
            Usuario? usuario = BuscarPorMatricula(matricula);

            if (usuario is null || usuario.PrimeiroAcessoConcluido)
            {
                return false;
            }

            usuario.ConcluirPrimeiroAcesso(nome, email);
            return true;
        }
    }
}
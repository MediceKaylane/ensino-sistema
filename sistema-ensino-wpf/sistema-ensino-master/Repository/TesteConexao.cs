
using System;
using System.Windows;
using sistema_ensino.data;
using sistema_ensino.model;
using sistema_ensino.controller;
using sistema_ensino.Repository;

namespace sistema_ensino.data
{
    public class TesteConexao
    {
        public static void Executar()
        {
            try
            {
                using (AppDbContext contexto = new AppDbContext())
                {
                    // Verifica a conexão com o banco.
                    bool conectado = contexto.Database.CanConnect();

                    if (!conectado)
                    {
                        MessageBox.Show("Não foi possível conectar ao banco.");
                        return;
                    }

                    string matricula = "A001";

                    // Verifica se o aluno já existe.
                    Aluno? aluno = contexto.Alunos.Find(matricula);

                    if (aluno is null)
                    {
                        aluno = new Aluno(matricula);

                        aluno.ConcluirPrimeiroAcesso(
                            "Aluno Teste",
                            "aluno@teste.com"
                        );

                        contexto.Alunos.Add(aluno);
                        contexto.SaveChanges();
                    }

                    // Instancia o Repository e o Controller.
                    AlunoRepository repository = new AlunoRepository(contexto);

                    AlunoController controller = new AlunoController(repository);

                    // Consulta o aluno pelo Controller.
                    Aluno? alunoEncontrado =
                        controller.BuscarAlunoPorMatricula(matricula);

                    if (alunoEncontrado is not null)
                    {
                        MessageBox.Show(
                            $"Teste realizado com sucesso!\n\n" +
                            $"Matrícula: {alunoEncontrado.Matricula}\n" +
                            $"Nome: {alunoEncontrado.Nome}\n" +
                            $"E-mail: {alunoEncontrado.Email}\n" +
                            $"Tipo: {alunoEncontrado.GetType().Name}"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Aluno não encontrado.");
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show($"Erro: {erro.Message}");
            }
        }
    }
}


using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
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
                    contexto.Database.Migrate();
                    bool conectado = contexto.Database.CanConnect();

                    if (!conectado)
                    {
                        MessageBox.Show("Não foi possível conectar ao banco.");
                        return;
                    }

                    string matricula = "A001";

                    Aluno? aluno = contexto.Alunos.Find(matricula);

                    if (aluno is null)
                    {
                        aluno = new Aluno(matricula);

                        contexto.Alunos.Add(aluno);
                        contexto.SaveChanges();
                    }

                    UsuarioController controller = new UsuarioController(
                        new UsuarioRepository(contexto));

                    Usuario? usuarioEncontrado =
                        controller.BuscarPorMatricula(matricula);

                    if (usuarioEncontrado is not null)
                    {
                        MessageBox.Show(
                            $"Teste realizado com sucesso!\n" +
                            $"Matrícula encontrada: {usuarioEncontrado.Matricula}\n" +
                            $"Tipo: {usuarioEncontrado.GetType().Name}"
                        );
                    }
                    else
                    {
                        MessageBox.Show("Usuário não encontrado.");
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

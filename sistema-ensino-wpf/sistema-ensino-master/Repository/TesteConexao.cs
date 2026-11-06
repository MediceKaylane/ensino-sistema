
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            StringBuilder relatorio = new StringBuilder();

            int aprovados = 0;
            int falhas = 0;

            void Testar(string nome, Action acao)
            {
                try
                {
                    acao();
                    aprovados++;
                    relatorio.AppendLine($"[OK] {nome}");
                }
                catch (Exception erro)
                {
                    falhas++;
                    relatorio.AppendLine($"[ERRO] {nome}: {erro.Message}");
                }
            }

            try
            {
                using (AppDbContext contexto = new AppDbContext())
                {
                    // 1. Conexão com o banco
                    Testar("Conexão com o banco", () =>
                    {
                        if (!contexto.Database.CanConnect())
                            throw new Exception("Banco inacessível.");
                    });

                    // 2. Tabelas
                    Testar("Tabela de usuários", () =>
                        contexto.Usuarios.Any());

                    Testar("Tabela de alunos", () =>
                        contexto.Alunos.Any());

                    Testar("Tabela de instrutores", () =>
                        contexto.Instrutores.Any());

                    Testar("Tabela de turmas", () =>
                        contexto.Turmas.Any());

                    Testar("Tabela de matérias", () =>
                        contexto.Materias.Any());

                    Testar("Tabela de atividades", () =>
                        contexto.Atividades.Any());

                    // 3. Categorias pré-cadastradas
                    Testar("Categorias de atividades", () =>
                    {
                        var categorias =
                            contexto.CategoriasAtividade.ToList();

                        if (categorias.Count < 5)
                            throw new Exception(
                                "As cinco categorias esperadas não foram encontradas."
                            );
                    });

                    // 4. Herança de Usuario
                    Testar("Herança de Aluno para Usuario", () =>
                    {
                        var aluno = new Aluno("TESTE_HERANCA");

                        if (!(aluno is Usuario))
                            throw new Exception(
                                "Aluno não herda de Usuario."
                            );
                    });

                    Testar("Herança de Instrutor para Usuario", () =>
                    {
                        var instrutor = contexto.Instrutores
                            .FirstOrDefault();

                        if (instrutor != null &&
                            !(instrutor is Usuario))
                        {
                            throw new Exception(
                                "Instrutor não herda de Usuario."
                            );
                        }
                    });

                    // 5. Relacionamento Aluno e Turma
                    Testar("Relacionamento entre Aluno e Turma", () =>
                    {
                        var alunos = contexto.Alunos
                            .Include(a => a.Turma)
                            .ToList();

                        foreach (var aluno in alunos)
                        {
                            if (aluno.Turma != null &&
                                !aluno.Turma.Alunos.Any(
                                    a => a.Matricula == aluno.Matricula))
                            {
                                throw new Exception(
                                    "Relacionamento inconsistente."
                                );
                            }
                        }
                    });

                    // 6. Relacionamento Instrutor e Turma
                    Testar("Relacionamento entre Instrutor e Turma", () =>
                    {
                        var instrutores = contexto.Instrutores
                            .Include(i => i.Turma)
                            .ToList();

                        foreach (var instrutor in instrutores)
                        {
                            if (instrutor.Turma != null &&
                                !instrutor.Turma.Instrutores.Any(
                                    i => i.Matricula == instrutor.Matricula))
                            {
                                throw new Exception(
                                    "Relacionamento inconsistente."
                                );
                            }
                        }
                    });

                    // 7. Relacionamento Turma e Materia
                    Testar("Relacionamento entre Turma e Materia", () =>
                    {
                        var turmas = contexto.Turmas
                            .Include(t => t.Materias)
                            .ToList();

                        foreach (var turma in turmas)
                        {
                            if (turma.Materias == null)
                                throw new Exception(
                                    "Coleção de matérias não carregada."
                                );
                        }
                    });

                    // 8. Relacionamento Instrutor e Atividades
                    Testar("Relacionamento entre Instrutor e Atividades", () =>
                    {
                        var instrutores = contexto.Instrutores
                            .Include(i => i.Atividades)
                            .ToList();

                        foreach (var instrutor in instrutores)
                        {
                            if (instrutor.Atividades == null)
                                throw new Exception(
                                    "Coleção de atividades não carregada."
                                );
                        }
                    });

                    // 9. Relacionamento Atividade e Categoria
                    Testar("Relacionamento entre Atividade e Categoria", () =>
                    {
                        var atividades = contexto.Atividades
                            .Include(a => a.Categoria)
                            .ToList();

                        foreach (var atividade in atividades)
                        {
                            if (atividade.Categoria == null)
                                throw new Exception(
                                    "Atividade sem categoria."
                                );
                        }
                    });

                    // 10. Consulta SQL direta por categoria
                    Testar("Consulta SQL direta - Prova de Lógica", () =>
                    {
                        var repositorio = new AtividadeRepository(contexto);
                        var controller = new AtividadeController(repositorio);

                        var atividades =
                            controller.ConsultarAtividadesPorCategoria(1);

                        var prova = atividades.FirstOrDefault(
                            a => a.Titulo == "Prova de Lógica"
                        );

                        if (prova == null)
                        {
                            throw new Exception(
                                "A atividade Prova de Lógica não foi encontrada na categoria Prova."
                            );
                        }
                    });
                }
            }
            catch (Exception erro)
            {
                falhas++;
                relatorio.AppendLine(
                    $"[ERRO GERAL] {erro.Message}"
                );
            }

            relatorio.AppendLine();
            relatorio.AppendLine("===== RESULTADO FINAL =====");
            relatorio.AppendLine($"Testes aprovados: {aprovados}");
            relatorio.AppendLine($"Testes com falha: {falhas}");

            MessageBox.Show(
                relatorio.ToString(),
                "Teste geral do sistema"
            );
        }
    }
}

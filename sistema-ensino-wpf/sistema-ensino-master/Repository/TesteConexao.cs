
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

                    Testar("Tabela de avaliações", () =>
                        contexto.Avaliacoes.Any());

                    Testar("Validação de publicação de atividade", () =>
                    {
                        var atividade = new Atividades("", "", "");
                        try
                        {
                            atividade.Publicar();
                            throw new Exception("A publicação aceitou instruções incompletas.");
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    });

                    Testar("Validação de vínculo de avaliação", () =>
                    {
                        var avaliacao = new Avaliacoes(
                            5m,
                            new Aluno("TESTE_AVALIACAO"),
                            new Atividades("Atividade de teste", "Descrição", "Link"));
                        try
                        {
                            avaliacao.Publicar();
                            throw new Exception("A avaliação aceitou uma atividade não persistida.");
                        }
                        catch (InvalidOperationException)
                        {
                        }
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

                    // 9. Relacionamentos da atividade
                    Testar("Relacionamentos entre atividade, instrutor e matéria", () =>
                    {
                        var atividades = contexto.Atividades
                            .Include(atividade => atividade.Instrutor)
                            .Include(atividade => atividade.Materia)
                            .ToList();

                        foreach (var atividade in atividades)
                        {
                            if (atividade.InstrutorMatricula is not null && atividade.Instrutor is null)
                                throw new Exception("Atividade com referência de instrutor inválida.");

                            if (atividade.MateriaId.HasValue && atividade.Materia is null)
                                throw new Exception("Atividade com referência de matéria inválida.");
                        }
                    });

                    Testar("Relacionamentos obrigatórios das avaliações", () =>
                    {
                        var avaliacoes = contexto.Avaliacoes
                            .Include(avaliacao => avaliacao.Aluno)
                            .Include(avaliacao => avaliacao.Atividade)
                            .ToList();

                        if (avaliacoes.Any(avaliacao =>
                            avaliacao.Aluno is null || avaliacao.Atividade is null))
                        {
                            throw new Exception("Avaliação sem aluno ou atividade.");
                        }
                    });

                    var repositorio = new AtividadeRepository(contexto);
                    var controller = new AtividadeController(repositorio);

                    Testar("Consulta de atividades por matéria via EF Core", () =>
                        controller.ConsultarAtividadesPorMateria(1));

                    Testar("Consulta de atividades por matéria via ADO.NET", () =>
                        controller.ConsultarAtividadesPorMateriaSemEF(1));
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

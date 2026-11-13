using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using sistema_ensino.controller;
using sistema_ensino.data;
using sistema_ensino.model;
using sistema_ensino.Repository;

namespace sistema_ensino
{
    public partial class PORTAL : Window
    {
        private readonly Usuario _usuario;

        public PORTAL(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            string perfil = usuario is Instrutor ? "Professor" : "Aluno";
            Identificacao.Text = $"{usuario.Nome}  •  {perfil}  •  Matrícula {usuario.Matricula}";
            Principal.Content = usuario is Instrutor ? "Turmas" : "Matérias";

            if (usuario is Instrutor instrutor)
            {
                AcoesInstrutor.Visibility = Visibility.Visible;
                MateriaAtividade.ItemsSource = instrutor.Turma?.Materias;
                AtividadeAvaliacao.ItemsSource = instrutor.Atividades;
                AlunoAvaliacao.ItemsSource = instrutor.Turma?.Alunos;
            }
            else
            {
                NotasAluno.Visibility = Visibility.Visible;
            }

            MostrarPrincipal(this, new RoutedEventArgs());
        }

        private void MostrarPrincipal(object sender, RoutedEventArgs e)
        {
            OcultarFormularios();
            if (_usuario is Instrutor instrutor)
            {
                string[] turmas = instrutor.Turma is Turma turma ? [$"Turma {turma.Id}  •  {turma.Tipo}"] : [];
                Exibir("Minhas turmas", "Turmas vinculadas à sua matrícula.", turmas, "Nenhuma turma vinculada.");
            }
            else
            {
                var materias = (_usuario as Aluno)?.Turma?.Materias.Select(item => item.Nome) ?? [];
                Exibir("Minhas matérias", "Matérias da sua turma.", materias, "Nenhuma matéria disponível.");
            }
        }

        private void MostrarAtividades(object sender, RoutedEventArgs e)
        {
            OcultarFormularios();
            IEnumerable<Atividades> atividades = _usuario switch
            {
                Instrutor instrutor => instrutor.Atividades,
                Aluno aluno => aluno.VisualizarAtividades(),
                _ => []
            };
            Exibir("Atividades", "Consulte as atividades disponíveis no seu perfil.",
                atividades.Select(item => item.Visualizar()), "Nenhuma atividade disponível.");
        }

        private void MostrarAvaliacoes(object sender, RoutedEventArgs e)
        {
            OcultarFormularios();
            if (_usuario is not Aluno aluno)
            {
                return;
            }

            try
            {
                using var contexto = new AppDbContext();
                var controller = new AlunoController(new AlunoRepository(contexto));
                var avaliacoes = controller.VisualizarAvaliacoes(aluno)
                    .Select(avaliacao => $"{avaliacao.Atividade.Titulo}\nNota: {avaliacao.Nota}");
                Exibir("Minhas notas", "Notas vinculadas à sua matrícula.", avaliacoes, "Nenhuma nota disponível.");
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                MensagemAcao.Text = "Não foi possível acessar as notas no banco de dados.";
            }
        }

        private void AbrirFormularioAtividade(object sender, RoutedEventArgs e)
        {
            ExibirFormulario(FormularioAtividade, "Publicar atividade", "Informe as instruções e a matéria da turma.");
        }

        private void AbrirFormularioAvaliacao(object sender, RoutedEventArgs e)
        {
            ExibirFormulario(FormularioAvaliacao, "Publicar notas", "Selecione uma atividade e um aluno da sua turma.");
        }

        private void SalvarAtividade(object sender, RoutedEventArgs e)
        {
            if (_usuario is not Instrutor instrutor || MateriaAtividade.SelectedItem is not Materia materia)
            {
                MensagemAcao.Text = "Selecione uma matéria da sua turma.";
                return;
            }

            try
            {
                var atividade = new Atividades(
                    TituloAtividade.Text.Trim(),
                    DescricaoAtividade.Text.Trim(),
                    ArquivoOuLinkAtividade.Text.Trim());
                using var contexto = new AppDbContext();
                var controller = new InstrutorController(new InstrutorRepository(contexto));
                controller.PublicarAtividade(instrutor.Matricula, atividade, materia.Id);
                AtividadeAvaliacao.ItemsSource = new InstrutorController(new InstrutorRepository(contexto))
                    .ConsultarAtividades(instrutor.Matricula);
                MensagemAcao.Foreground = System.Windows.Media.Brushes.DarkGreen;
                MensagemAcao.Text = "Atividade publicada com sucesso.";
            }
            catch (ArgumentException erro)
            {
                MensagemAcao.Text = erro.Message;
            }
            catch (InvalidOperationException erro)
            {
                MensagemAcao.Text = erro.Message;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                MensagemAcao.Text = "Não foi possível salvar a atividade no banco de dados.";
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                MensagemAcao.Text = "Não foi possível acessar o banco de dados.";
            }
        }

        private void SalvarAvaliacao(object sender, RoutedEventArgs e)
        {
            if (_usuario is not Instrutor instrutor ||
                AlunoAvaliacao.SelectedItem is not Aluno aluno ||
                AtividadeAvaliacao.SelectedItem is not Atividades atividade)
            {
                MensagemAcao.Text = "Selecione uma atividade e um aluno da sua turma.";
                return;
            }

            if (!decimal.TryParse(NotaAvaliacao.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal nota))
            {
                MensagemAcao.Text = "Informe uma nota numérica válida.";
                return;
            }

            try
            {
                using var contexto = new AppDbContext();
                var controller = new InstrutorController(new InstrutorRepository(contexto));
                controller.PublicarAvaliacao(instrutor.Matricula, aluno.Matricula, atividade.Id, nota);
                MensagemAcao.Foreground = System.Windows.Media.Brushes.DarkGreen;
                MensagemAcao.Text = "Nota publicada com sucesso.";
            }
            catch (ArgumentException erro)
            {
                MensagemAcao.Text = erro.Message;
            }
            catch (InvalidOperationException erro)
            {
                MensagemAcao.Text = erro.Message;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                MensagemAcao.Text = "Não foi possível salvar a nota no banco de dados.";
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                MensagemAcao.Text = "Não foi possível acessar o banco de dados.";
            }
        }

        private void MostrarPerfil(object sender, RoutedEventArgs e)
        {
            OcultarFormularios();
            Exibir("Meu perfil", "Informações vinculadas ao seu acesso.", [_usuario.ConsultarPerfil()], "");
        }

        private void ExibirFormulario(System.Windows.Controls.StackPanel formulario, string secao, string descricao)
        {
            OcultarFormularios();
            formulario.Visibility = Visibility.Visible;
            Itens.ItemsSource = null;
            Secao.Text = secao;
            Descricao.Text = descricao;
            MensagemAcao.Text = string.Empty;
            MensagemAcao.Foreground = System.Windows.Media.Brushes.Firebrick;
        }

        private void OcultarFormularios()
        {
            FormularioAtividade.Visibility = Visibility.Collapsed;
            FormularioAvaliacao.Visibility = Visibility.Collapsed;
            MensagemAcao.Text = string.Empty;
        }

        private void Exibir(string secao, string descricao, IEnumerable<string> itens, string vazio)
        {
            Secao.Text = secao;
            Descricao.Text = descricao;
            Itens.ItemsSource = itens.DefaultIfEmpty(vazio).ToArray();
        }

        private void Sair(object sender, RoutedEventArgs e)
        {
            var login = new LoginForm();
            Application.Current.MainWindow = login;
            login.Show();
            Close();
        }
    }
}

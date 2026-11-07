using System.Net.Mail;
using System.Windows;
using sistema_ensino.controller;
using sistema_ensino.data;
using sistema_ensino.Repository;

namespace sistema_ensino
{
    public partial class LoginForm : Window
    {
        private bool _primeiroAcesso;

        public LoginForm()
        {
            InitializeComponent();
            Loaded += (_, _) => Matricula.Focus();
        }

        private void AlternarAcesso(object sender, RoutedEventArgs e)
        {
            ExibirModo(!_primeiroAcesso);
        }

        private void ExibirModo(bool primeiroAcesso)
        {
            _primeiroAcesso = primeiroAcesso;
            Titulo.Text = primeiroAcesso ? "Primeiro acesso" : "Acesse o portal";
            Subtitulo.Text = primeiroAcesso
                ? "Use sua matrícula pré-cadastrada e crie uma senha de pelo menos 8 caracteres."
                : "Entre com sua matrícula e senha.";
            DadosPrimeiroAcesso.Visibility = primeiroAcesso ? Visibility.Visible : Visibility.Collapsed;
            DadosConfirmacao.Visibility = DadosPrimeiroAcesso.Visibility;
            Acao.Content = primeiroAcesso ? "Criar acesso" : "Entrar";
            Alternar.Content = primeiroAcesso ? "Voltar ao login" : "Primeiro acesso";
            Recuperar.Visibility = primeiroAcesso ? Visibility.Collapsed : Visibility.Visible;
            Mensagem.Text = string.Empty;
            Senha.Clear();
            Confirmacao.Clear();
            Matricula.Focus();
        }

        private async void ExecutarAcesso(object sender, RoutedEventArgs e)
        {
            string matricula = Matricula.Text.Trim();
            string senha = Senha.Password;
            string nome = Nome.Text.Trim();
            string email = Email.Text.Trim();
            bool primeiroAcesso = _primeiroAcesso;
            if (matricula.Length == 0 || senha.Length == 0)
            {
                Mensagem.Text = "Informe a matrícula e a senha.";
                return;
            }

            if (primeiroAcesso && (nome.Length == 0 || !MailAddress.TryCreate(email, out _) ||
                senha.Length < 8 || senha != Confirmacao.Password))
            {
                Mensagem.Text = "Confira o nome, o e-mail e a confirmação da senha de pelo menos 8 caracteres.";
                return;
            }

            Acao.IsEnabled = Alternar.IsEnabled = Recuperar.IsEnabled = false;
            Mensagem.Text = "Aguarde…";
            try
            {
                if (primeiroAcesso)
                {
                    bool criado = await Task.Run(() =>
                    {
                        using var banco = new AppDbContext();
                        var controller = new UsuarioController(new UsuarioRepository(banco));
                        return controller.DefinirSenha(matricula, nome, email, senha);
                    });

                    if (criado)
                    {
                        ExibirModo(false);
                        Senha.Focus();
                    }

                    Mensagem.Text = criado
                        ? "Acesso criado. Entre com a sua senha."
                        : "Confira a matrícula. Ela pode não estar cadastrada ou já ter uma senha.";
                    return;
                }

                var usuario = await Task.Run(() =>
                {
                    using var banco = new AppDbContext();
                    var controller = new UsuarioController(new UsuarioRepository(banco));
                    return controller.Autenticar(matricula, senha);
                });
                if (usuario is null)
                {
                    Mensagem.Text = "Matrícula ou senha inválida. Se ainda não criou sua senha, use Primeiro acesso.";
                    return;
                }

                var portal = new PORTAL(usuario);
                Application.Current.MainWindow = portal;
                portal.Show();
                Close();
            }
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                Mensagem.Text = "Não foi possível acessar o banco do projeto. Verifique se ele está disponível.";
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                Mensagem.Text = "Não foi possível salvar o primeiro acesso. Tente novamente.";
            }
            finally
            {
                Acao.IsEnabled = Alternar.IsEnabled = Recuperar.IsEnabled = true;
            }
        }

        private void OrientarRecuperacao(object sender, RoutedEventArgs e)
        {
            Mensagem.Text = "A recuperação de senha ainda não está disponível neste portal.";
        }
    }
}

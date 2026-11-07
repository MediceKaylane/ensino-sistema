using System.Windows;
using sistema_ensino.model;

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
            MostrarPrincipal(this, new RoutedEventArgs());
        }

        private void MostrarPrincipal(object sender, RoutedEventArgs e)
        {
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
            IEnumerable<Atividade> atividades = _usuario switch
            {
                Instrutor instrutor => instrutor.Atividades,
                Aluno aluno => aluno.VisualizarAtividades(),
                _ => []
            };
            Exibir("Atividades", "Consulte as atividades disponíveis no seu perfil.",
                atividades.Select(item => item.Visualizar()), "Nenhuma atividade disponível.");
        }

        private void MostrarPerfil(object sender, RoutedEventArgs e)
        {
            Exibir("Meu perfil", "Informações vinculadas ao seu acesso.", [_usuario.ConsultarPerfil()], "");
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

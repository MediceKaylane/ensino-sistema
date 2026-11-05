using System.Windows;
using sistema_ensino.data;

namespace sistema_ensino
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            TesteConexao.Executar();
            base.OnStartup(e);
        }
    }
}

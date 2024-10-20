using MaimaiConsulationCenter.View;
using System.Windows;

namespace MaimaiConsulationCenter
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (new LoginView().ShowDialog() == true)
            {
                new MainView().ShowDialog();
            }
            Application.Current.Shutdown();

        }
    }
}

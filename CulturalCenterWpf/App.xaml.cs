using CulturalCenterWpf.Models;
using CulturalCenterWpf.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CulturalCenterWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppSettings Settings { get; private set; } = new();

        protected async override void OnStartup(StartupEventArgs e)
        {
            SettingService settingService = new();
            Settings = await settingService.LoadJsonAsync();

            base.OnStartup(e);
        }
    }

}

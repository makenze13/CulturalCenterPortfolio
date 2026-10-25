using CulturalCenterWpf.Models;
using Stimulsoft.Controls.Wpf;
using System.IO;
using System.Text.Json;

namespace CulturalCenterWpf.Services
{
    public class SettingService
    {
        private readonly string _settingsPath;
    
        public SettingService()
        {
            _settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings", "settings.json"); //получаем путь к файлу
        }
        
        //получаем данные с json файла
        public async Task<AppSettings> LoadJsonAsync()
        {
            try
            {
                //if (!File.Exists(_settingsPath)) //проверяем что файл был получен
                //    return null;

                string json = await File.ReadAllTextAsync(_settingsPath); //читаем файл в переменную

                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings(); //возвращаем переменную как модель для настроек
            }
            catch (Exception ex)
            {
                await AddAndShowError(ex);
                return new AppSettings();
            }
        }

        //записываем данные в json файл
        public async Task SaveJsonAsync(AppSettings appSettings)
        {
            try
            {
                string json = JsonSerializer.Serialize(appSettings); //преобразуем модель в текст

                await File.WriteAllTextAsync(_settingsPath, json); //записываем текст в файл
            }
            catch (Exception ex)
            {
                await AddAndShowError(ex);
            }
            
        }

        public async Task AddAndShowError(Exception ex)
        {
            LogsErrorService logsErrorService = new();
            await logsErrorService.AddLogsError(null, ex.Message); //добавляем ошибку в бд

            LogsErrorService.ShowErrorMessage(ex.Message); //выводим ошибку
        }
    }
}

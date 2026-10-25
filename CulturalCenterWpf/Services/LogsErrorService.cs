using CulturalCenterWpf.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CulturalCenterWpf.Services
{
    internal class LogsErrorService
    {
        private readonly LogsErrorApiService _logsErrorApiService = new();

        //добавляем логи ошибок
        public async Task AddLogsError(int? idUser, string errorDescription, string errorSystem = "Настольное приложение")
        {
            LogsError logsError = new()
            {
                IdUser = idUser == 0 ? null : idUser,
                ErrorDescription = errorDescription,
                ErrorDateTime = DateTime.Now,
                ErrorSystem = errorSystem
            };

            await _logsErrorApiService.PostLogsErrorAsync(logsError);
        }

        //вывод ошибки пользователю
        public static void ShowErrorMessage(string errorMessage)
        {
            MessageBox.Show($"Ошбика: {errorMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //вывод предупреждения пользователю
        public static void ShowWarningMessage(string warningMessage, string caption)
        {
            MessageBox.Show(warningMessage, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}

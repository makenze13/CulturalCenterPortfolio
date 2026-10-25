using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulturalCenterWpf.Services
{
    internal class LogsActionService
    {
        private readonly LogsActionApiService _logsActionApiService = new();

        //добавляем логи действий
        public async Task AddLogsAction(int idUser, string action, string logsObject, string actionSystem = "Настольное приложение")
        {
            LogsAction logsAction = new()
            {
                IdUser = idUser,
                Action = action,
                Object = logsObject,
                ActionDateTime = DateTime.Now,
                ActionSystem = actionSystem
            };

            await _logsActionApiService.PostLogsActionAsync(logsAction);
        }
    }
}

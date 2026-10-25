using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CulturalCenterWpf.Services
{
    internal class LogsActionApiService
    {
        private readonly ApiServices _apiService = new ApiServices();
        private readonly HttpClient _httpClient;

        public LogsActionApiService()
        {
            _httpClient = _apiService.GetHttpClient();
        }

        public async Task<List<LogsAction>> GetLogsActionAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<LogsAction>>("LogsActions") ?? new List<LogsAction>();
        }

        public async Task<LogsAction?> PostLogsActionAsync(LogsAction logsAction)
        {
            var response = await _httpClient.PostAsJsonAsync<LogsAction>("LogsActions", logsAction);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LogsAction>();
        }
    }
}

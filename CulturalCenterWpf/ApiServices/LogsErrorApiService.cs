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
    internal class LogsErrorApiService
    {
        private readonly ApiServices _apiService = new ApiServices();
        private readonly HttpClient _httpClient;

        public LogsErrorApiService()
        {
            _httpClient = _apiService.GetHttpClient();
        }

        public async Task<List<LogsError>> GetLogsErrorAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<LogsError>>("LogsErrors") ?? new List<LogsError>();
        }

        public async Task<LogsError?> PostLogsErrorAsync(LogsError logsError)
        {
            var response = await _httpClient.PostAsJsonAsync<LogsError>("LogsErrors", logsError);

            if (!response.IsSuccessStatusCode)
                return null;
        
            return await response.Content.ReadFromJsonAsync<LogsError>();
        }
    }
}

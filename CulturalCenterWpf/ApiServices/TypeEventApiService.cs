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
    internal class TypeEventApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiServices _apiServices = new ApiServices();

        public TypeEventApiService()
        {
            _httpClient = _apiServices.GetHttpClient();
        }

        public async Task<List<TypesEvent>> GetTypesEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TypesEvent>>("TypesEvents") ?? new List<TypesEvent>();
        }

        public async Task<TypesEvent?> PostTypeEventAsync(TypesEvent typesEvent)
        {
            var response = await _httpClient.PostAsJsonAsync<TypesEvent?>("TypesEvents", typesEvent);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<TypesEvent?>();
        }

        public async Task<bool> DeleteTypeEventAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"TypesEvents/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}

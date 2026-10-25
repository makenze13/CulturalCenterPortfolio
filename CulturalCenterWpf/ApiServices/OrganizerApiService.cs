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
    internal class OrganizerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiServices _apiServices = new ApiServices();

        public OrganizerApiService()
        {
            _httpClient = _apiServices.GetHttpClient();
        }

        public async Task<List<Organizer>> GetOrganizerAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Organizer>>("Organizers") ?? new List<Organizer>();
        }

        public async Task<Organizer?> PostOrganizerAsync(Organizer organizer)
        {
            var response = await _httpClient.PostAsJsonAsync<Organizer?>("Organizers", organizer);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Organizer?>();
        }

        public async Task<Organizer?> PutOrganizerAsync(int id,Organizer organizer)
        {
            var response = await _httpClient.PutAsJsonAsync<Organizer>($"Organizers/{id}", organizer);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Organizer?>();
        }

        public async Task<bool> DeleteOrganizerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Organizers/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}

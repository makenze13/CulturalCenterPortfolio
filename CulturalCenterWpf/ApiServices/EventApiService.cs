using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace CulturalCenterWpf.Services
{
    internal class EventApiService
    {
        private readonly ApiServices _apiServices = new ApiServices();
        private readonly HttpClient _httpClient;

        public EventApiService()
        {
            _httpClient = _apiServices.GetHttpClient();
        }
        
        public async Task<List<EventDTO>> GetEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EventDTO>>("Events/EventsDTO") ?? new List<EventDTO>();
        }

        public async Task<List<EventDTO>> GetEventsWithParameters(string? search, string? filterType, string? sortBy, DateTime? filterDate, string? sortDirection = "asc", bool isRelevant = true)
        {
            var urlSearch = $"?search={search}";
            var urlfilterType = $"&filterType={filterType}";
            var urlSortBy = $"&sortBy={sortBy}";
            var urlFilterDate = $"&filterDate={filterDate:yyyy-MM-dd}";
            var urlSortDirection = $"&sortDirection={sortDirection}";
            var urlIsRelevant = $"&isRelevant={isRelevant}";

            var url = "Events/search" + urlSearch + urlfilterType + urlSortBy + urlFilterDate + urlSortDirection + urlIsRelevant;

            return await _httpClient.GetFromJsonAsync<List<EventDTO>>(url) ?? new List<EventDTO>();
        }

        public async Task<Event?> PostEventAsync(Event @event)
        {
            var response = await _httpClient.PostAsJsonAsync("Events", @event);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Event?>();
        } 

        public async Task<Event?> PutEventAsync(int id, Event @event)
        {
            var response = await _httpClient.PutAsJsonAsync($"Events/{id}", @event);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Event?>();
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Events/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Seat>> CreateSectorAsync(int idEvent, string sector, EventDTO @event)
        {
            var url = $"Events/{idEvent}/seats/{sector}";

            List<Seat> seats = await _httpClient.GetFromJsonAsync<List<Seat>>(url) ?? new List<Seat>();
            
            foreach (var seat in seats)
                seat.SeatEventDTO = @event;

            return seats;
        } 
    }
}

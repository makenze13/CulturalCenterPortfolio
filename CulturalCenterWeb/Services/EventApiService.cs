using CulturalCenterWeb.Models;
using System.Diagnostics.Tracing;
using System.Net.Http.Json;

namespace CulturalCenterWeb.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EventDTO>?> GetEventsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<EventDTO>?>("Events/EventsDTO") ?? new List<EventDTO>();
        }

        public async Task<List<EventDTO>> GetEventsWithParameters(string? filterType)
        {
            string urlfilterType;

            if (filterType != null)
                urlfilterType = $"?filterType={filterType}";
            else
                urlfilterType = "";

            var url = "Events/search" + urlfilterType;

            return await _httpClient.GetFromJsonAsync<List<EventDTO>>(url) ?? new List<EventDTO>();
        }

        public async Task<EventDTO?> GetEventIdAsync(int id)
        {
            var eventList = await _httpClient.GetFromJsonAsync<List<EventDTO>?>("Events/EventsDTO");

            var eventDto = eventList.FirstOrDefault(e => e.IdEvent == id);
            
            return eventDto;
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

using CulturalCenterWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CulturalCenterWpf.Services
{
    internal class TicketApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiServices _apiService = new ApiServices();

        public TicketApiService()
        {
            _httpClient = _apiService.GetHttpClient();
        }

        public async Task<Ticket?> GetSeatTicketAsync(int idEvent, Seat seat)
        {
            var ticketSeat = seat.Sector + "-" + seat.Row + "-" + seat.Number;

            var url = "Tickets/seat-ticket" +
                      $"?idEvent={idEvent}" +
                      $"&seat={ticketSeat}";

            return await _httpClient.GetFromJsonAsync<Ticket?>(url);
        }

        public async Task<Ticket?> PostTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PostAsJsonAsync<Ticket?>("Tickets", ticket);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Ticket?>();
        } 
    }
}

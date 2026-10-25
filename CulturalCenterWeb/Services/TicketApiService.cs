using CulturalCenterWeb.Models;

namespace CulturalCenterWeb.Services
{
    public class TicketApiService
    {
        private readonly HttpClient _httpClient;

        public TicketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Ticket?> GetSeatTicketAsync(int idEvent, Seat seat)
        {
            var ticketSeat = seat.Sector + "-" + seat.Row + "-" + seat.Number;

            var url = "Tickets/seat-ticket" +
                      $"?idEvent={idEvent}" +
                      $"&seat={ticketSeat}";

            return await _httpClient.GetFromJsonAsync<Ticket?>(url);
        }

        public async Task<List<Ticket?>> GetUserTicketsAsync(int idUser)
        {
            return await _httpClient.GetFromJsonAsync<List<Ticket>>($"Tickets/UserTickets?idUser={idUser}");
        }
        public async Task<Ticket?> PostTicketAsync(Ticket ticket)
        {
            var response = await _httpClient.PostAsJsonAsync<Ticket?>("Tickets", ticket);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Ticket?>();
        }

        public async Task<Ticket?> GetIdTicketAsync(int idTicket)
        {
            return await _httpClient.GetFromJsonAsync<Ticket?>($"Tickets/{idTicket}");
        }
    }
}

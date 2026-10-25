using CulturalCenterApi.Models;
using CulturalCenterWpf.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;

namespace CulturalCenterApi.Services
{
    public class EventService
    {
        private CulturalCenterContext _context;

        public EventService(CulturalCenterContext context)
        {
            _context = context;
        }

        public async Task<List<Seat>> GetSectorSeatsAsync(int idEvent, string sector, decimal priceMultiplier)
        {
            var seats = new List<Seat>();
            
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.IdEvent == idEvent);

            decimal basePrice = @event.AvgTicketPrice;

            var eventTickets = await _context.Tickets.Where(t => t.IdEvent == idEvent).ToListAsync();

            for (int row = 1; row <= 15; row++)
            {
                for (int number = 1; number <= 10; number++)
                {
                    var currentSeat = sector + "-" + row + "-" + number;

                    var seat = eventTickets.FirstOrDefault(t => t.Seat == currentSeat);

                    seats.Add(new Seat
                    {
                        Sector = sector,
                        Row = row,
                        Number = number,
                        Price = basePrice * priceMultiplier,
                        Status = seat == null ? SeatStatus.Free : SeatStatus.Sold
                    });
                }
            }

            return seats;
        }

        public async Task<List<EventDTO>> GetEventsWith(string search, string filterType, string sortDirection = "asc", string? sortBy = null, DateTime? filterDate = null, bool isRelevant = true)
        {
            var today = DateTime.Today;

            var events = _context.Events.Select(e => new EventDTO
            {
                IdEvent = e.IdEvent,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                AvgTicketPrice = e.AvgTicketPrice,
                IdOrganizer = e.IdOrganizer,
                OrganizerName = e.IdOrganizerNavigation.OrganizerName,
                IdTypeEvent = e.IdTypeEvent,
                TypeEvent = e.IdTypeEventNavigation.TypeEvent
            }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                events = events.Where(e => EF.Functions.ILike(e.EventName, $"%{search}%"));

            if (!string.IsNullOrWhiteSpace(filterType))
            {
                var typeEvent = await _context.TypesEvents.Where(ty => ty.TypeEvent == filterType).FirstOrDefaultAsync();
                if (typeEvent != null)
                    events = events.Where(e => e.IdTypeEvent == typeEvent.IdTypeEvent);
            }
            
            if (filterDate != null)
                events = events.Where(e => e.EventDate.Date == filterDate.Value.Date);
            else if(isRelevant)
                events = events.Where(e => e.EventDate >= today);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "date":
                        events = sortDirection == "asc" ? events.OrderBy(e => e.EventDate) : events.OrderByDescending(e => e.EventDate);
                        break;
                    case "name":
                        events = sortDirection == "asc" ? events.OrderBy(e => e.EventName) : events.OrderByDescending(e => e.EventName);
                        break;
                    case "price":
                        events = sortDirection == "asc" ? events.OrderBy(e => e.AvgTicketPrice) : events.OrderByDescending(e => e.AvgTicketPrice);
                        break;
                }
            }

            return await events.ToListAsync();
        }
    }
}

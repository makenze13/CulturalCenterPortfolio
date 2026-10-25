using CulturalCenterApi.Models;
using CulturalCenterApi.Services;
using CulturalCenterWpf.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly CulturalCenterContext _context;
        private readonly EventService _eventService;

        public EventsController(CulturalCenterContext context, EventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEventAsync()
        {
            var eventsList = await _context.Events.ToListAsync();

            return Ok(eventsList);
        }

        [HttpGet("EventsDTO")]
        public async Task<ActionResult<List<EventDTO>>> GetEventDTOAsync()
        {
            var eventsDTOList = await _context.Events.Select(e => new EventDTO
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
            }).ToListAsync();

            return Ok(eventsDTOList);
        }

        [HttpGet("{idEvent}")]
        public async Task<ActionResult<Event>> GetEventIdAsync(int idEvent)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.IdEvent == idEvent);

            if (eventItem == null)
                return NotFound();

            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> PostEventAsync([FromBody] Event eventBody)
        {
            if (eventBody == null)
                return BadRequest();

            await _context.Events.AddAsync(eventBody);

            await _context.SaveChangesAsync();

            return Ok(eventBody);
        }

        [HttpPut("{idEvent}")]
        public async Task<ActionResult<Event>> PutEventAsync(int idEvent, [FromBody] Event eventBody)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.IdEvent == idEvent);

            if (eventItem == null)
                return NotFound();

            if (eventBody == null)
                return BadRequest();

            eventItem.IdOrganizer = eventBody.IdOrganizer;
            eventItem.EventDescription = eventBody.EventDescription;
            eventItem.IdTypeEvent = eventBody.IdTypeEvent;
            eventItem.AvgTicketPrice = eventBody.AvgTicketPrice;
            eventItem.EventDate = eventBody.EventDate;
            eventItem.EventName = eventBody.EventName;

            await _context.SaveChangesAsync();

            return Ok(eventItem);
        }

        [HttpDelete("{idEvent}")]
        public async Task<IActionResult> DeleteEventAsync(int idEvent)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(e => e.IdEvent == idEvent);

            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{idEvent}/seats/{sector}")]
        public async Task<ActionResult<List<Seat>>> GetSectorSeatsAsync(int idEvent, string sector)
        {
            if (string.IsNullOrWhiteSpace(sector))
                return BadRequest();

            if (sector != "C1" && sector != "C2" && sector != "C3")
                return BadRequest("Неизвестный сектор");

            decimal sectorPrice = sector == "C2" ? 1.2m : 0.9m;

            var seats = await _eventService.GetSectorSeatsAsync(idEvent, sector, sectorPrice);

            return Ok(seats);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<EventDTO>>> GetEventsWithParameters(string? search, string? filterType, string? sortBy, DateTime? filterDate, string? sortDirection = "asc", bool isRelevant = true)
        {
            var events = await _eventService.GetEventsWith(search, filterType, sortDirection, sortBy, filterDate, isRelevant);

            return Ok(events);
        }
    }
}

using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly CulturalCenterContext _context;

        public TicketsController(CulturalCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTicketAsync()
        {
            var ticketsList = await _context.Tickets.ToListAsync();

            return Ok(ticketsList);
        }

        [HttpGet("UserTickets")]
        public async Task<ActionResult<List<Ticket>>> GetUserTicketsAsync(int idUser)
        {
            var ticketList = await _context.Tickets.Where(t => t.IdUser == idUser).ToListAsync();

            return Ok(ticketList);
        }

        [HttpGet("{idTicket}")]
        public async Task<ActionResult<Ticket>> GetTicketIdAsync(int idTicket)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.IdTicket == idTicket);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpGet("seat-ticket")]
        public async Task<ActionResult<Ticket>> GetSeatTicketAsync([FromQuery] int idEvent, [FromQuery] string seat)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Seat == seat && t.IdEvent == idEvent);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicketAsync([FromBody] Ticket ticketBody)
        {
            if (ticketBody == null)
                return BadRequest();

            await _context.Tickets.AddAsync(ticketBody);

            await _context.SaveChangesAsync();

            return Ok(ticketBody);
        }

        [HttpPut("{idTicket}")]
        public async Task<ActionResult<Ticket>> PutTicketAsync(int idTicket, [FromBody] Ticket ticketBody)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.IdTicket == idTicket);

            if (ticket == null)
                return NotFound();

            if (ticketBody == null)
                return BadRequest();

            _context.Entry(ticket).CurrentValues.SetValues(ticketBody);

            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete("{idTicket}")]
        public async Task<IActionResult> DeleteTicketAsync(int idTicket)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.IdTicket == idTicket);

            if (ticket == null)
                return NotFound();

            _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

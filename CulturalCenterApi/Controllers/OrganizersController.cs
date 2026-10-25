using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizersController : ControllerBase
    {
        private readonly CulturalCenterContext _context;

        public OrganizersController(CulturalCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Organizer>>> GetOrganizerAsync()
        {
            var organizersList = await _context.Organizers.ToListAsync();

            return Ok(organizersList);
        }

        [HttpGet("{idOrganizer}")]
        public async Task<ActionResult<Organizer>> GetOrganizerIdAsync(int idOrganizer)
        {
            var organizer = await _context.Organizers.FirstOrDefaultAsync(o => o.IdOrganizer == idOrganizer);

            if (organizer == null)
                return NotFound();

            return Ok(organizer);
        }

        [HttpPost]
        public async Task<ActionResult<Organizer>> PostOrganizerAsync([FromBody] Organizer organizerBody)
        {
            if (organizerBody == null)
                return BadRequest();

            await _context.Organizers.AddAsync(organizerBody);

            await _context.SaveChangesAsync();

            return Ok(organizerBody);
        }

        [HttpPut("{idOrganizer}")]
        public async Task<ActionResult<Organizer>> PutOrganizerAsync(int idOrganizer, [FromBody] Organizer organizerBody)
        {
            var organizer = await _context.Organizers.FirstOrDefaultAsync(o => o.IdOrganizer == idOrganizer);

            if (organizer == null)
                return NotFound();

            if (organizerBody == null)
                return BadRequest();

            organizer.OrganizerName = organizerBody.OrganizerName;
            organizer.OrganizerNumberPhone = organizerBody.OrganizerNumberPhone;
            organizer.OrganizerEmail = organizerBody.OrganizerEmail;
            organizer.OrganizerDescription = organizerBody.OrganizerDescription;

            await _context.SaveChangesAsync();

            return Ok(organizer);
        }

        [HttpDelete("{idOrganizer}")]
        public async Task<IActionResult> DeleteOrganizerAsync(int idOrganizer)
        {
            var organizer = await _context.Organizers.FirstOrDefaultAsync(o => o.IdOrganizer == idOrganizer);

            if (organizer == null)
                return NotFound();

            _context.Organizers.Remove(organizer);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

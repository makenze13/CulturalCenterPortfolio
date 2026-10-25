using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesEventsController : ControllerBase
    {
        private readonly CulturalCenterContext _context;

        public TypesEventsController(CulturalCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TypesEvent>>> GetTypesEventAsync()
        {
            var typesEventsList = await _context.TypesEvents.ToListAsync();

            return Ok(typesEventsList);
        }

        [HttpGet("{idTypesEvent}")]
        public async Task<ActionResult<TypesEvent>> GetTypesEventIdAsync(int idTypesEvent)
        {
            var typesEvent = await _context.TypesEvents.FirstOrDefaultAsync(te => te.IdTypeEvent == idTypesEvent);

            if (typesEvent == null)
                return NotFound();

            return Ok(typesEvent);
        }

        [HttpPost]
        public async Task<ActionResult<TypesEvent>> PostTypesEventAsync([FromBody] TypesEvent typesEventBody)
        {

            if (typesEventBody == null)
                return BadRequest();

            await _context.TypesEvents.AddAsync(typesEventBody);

            await _context.SaveChangesAsync();

            return Ok(typesEventBody);
        }

        [HttpPut("{idTypesEvent}")]
        public async Task<ActionResult<TypesEvent>> PutTypesEventAsync(int idTypesEvent, [FromBody] TypesEvent typesEventBody)
        {
            var typesEvent = await _context.TypesEvents.FirstOrDefaultAsync(te => te.IdTypeEvent == idTypesEvent);

            if (typesEvent == null)
                return NotFound();

            if (typesEventBody == null)
                return BadRequest();

            _context.Entry(typesEvent).CurrentValues.SetValues(typesEventBody);

            await _context.SaveChangesAsync();

            return Ok(typesEvent);
        }

        [HttpDelete("{idTypesEvent}")]
        public async Task<IActionResult> DeleteTypesEventAsync(int idTypesEvent)
        {
            var typesEvent = await _context.TypesEvents.FirstOrDefaultAsync(te => te.IdTypeEvent == idTypesEvent);

            if (typesEvent == null)
                return NotFound();

            _context.TypesEvents.Remove(typesEvent);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

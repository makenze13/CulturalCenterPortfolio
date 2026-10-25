using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsActionsController : ControllerBase
    {
        private readonly CulturalCenterContext _context;

        public LogsActionsController(CulturalCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<LogsAction>>> GetLogsActionAsync()
        {
            var logsActionList = await _context.LogsActions.ToListAsync();

            return Ok(logsActionList);
        }

        [HttpGet("{idLogsAction}")]
        public async Task<ActionResult<LogsAction>> GetLogsActionIdAsync(int idLogsAction)
        {
            var logsAction = await _context.LogsActions.FirstOrDefaultAsync(la => la.IdRecordLogAction == idLogsAction);

            if (logsAction == null)
                return NotFound();

            return Ok(logsAction);
        }

        [HttpPost]
        public async Task<ActionResult<LogsAction>> PostLogsActionAsync([FromBody] LogsAction logsActionBody)
        {
            if (logsActionBody == null)
                return BadRequest();

            await _context.LogsActions.AddAsync(logsActionBody);

            await _context.SaveChangesAsync();

            return Ok(logsActionBody);
        }

        [HttpPut("{idLogsAction}")]
        public async Task<ActionResult<LogsAction>> PutLogsActionAsync(int idLogsAction, [FromBody] LogsAction logsActionBody)
        {
            var logsAction = await _context.LogsActions.FirstOrDefaultAsync(la => la.IdRecordLogAction == idLogsAction);

            if (logsAction == null)
                return NotFound();

            if (logsActionBody == null)
                return BadRequest();

            _context.Entry(logsAction).CurrentValues.SetValues(logsActionBody);

            await _context.SaveChangesAsync();

            return Ok(logsAction);
        }

        [HttpDelete("{idLogsAction}")]
        public async Task<ActionResult<LogsAction>> DeleteLogsActionAsync(int idLogsAction)
        {
            var logsAction = await _context.LogsActions.FirstOrDefaultAsync(la => la.IdRecordLogAction == idLogsAction);

            if (logsAction == null)
                return NotFound();

            _context.LogsActions.Remove(logsAction);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

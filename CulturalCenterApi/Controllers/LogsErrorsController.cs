using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsErrorsController : ControllerBase
    {
        private readonly CulturalCenterContext _context;

        public LogsErrorsController (CulturalCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<LogsError>>> GetLogsErrorAsync()
        {
            var logsErrorList = await _context.LogsErrors.ToListAsync();

            return Ok(logsErrorList);
        }

        [HttpGet("{idLogsError}")]
        public async Task<ActionResult<LogsError>> GetLogsErrorIdAsync(int idLogsError)
        {
            var logsError = await _context.LogsErrors.FirstOrDefaultAsync(le => le.IdRecordLogError == idLogsError);

            if (logsError == null)
                return NotFound();

            return Ok(logsError);
        }

        [HttpPost]
        public async Task<ActionResult<LogsError>> PostLogsErrorAsync([FromBody] LogsError logsErrorBody)
        {
            if (logsErrorBody == null)
                return BadRequest();

            await _context.LogsErrors.AddAsync(logsErrorBody);

            await _context.SaveChangesAsync();

            return Ok(logsErrorBody);
        }

        [HttpPut("{idLogsError}")]
        public async Task<ActionResult<LogsError>> PutLogsErrorAsync(int idLogsError, [FromBody] LogsError logsErrorBody)
        {
            var logsError = await _context.LogsErrors.FirstOrDefaultAsync(le => le.IdRecordLogError == idLogsError);

            if (logsError == null)
                return NotFound();

            if (logsErrorBody == null)
                return BadRequest();

            _context.Entry(logsError).CurrentValues.SetValues(logsErrorBody);

            await _context.SaveChangesAsync();

            return Ok(logsError);
        }

        [HttpDelete("{idLogsError}")]
        public async Task<IActionResult> DeleteLogsErrorAsync(int idLogsError)
        {
            var logsError = await _context.LogsErrors.FirstOrDefaultAsync(le => le.IdRecordLogError == idLogsError);

            if (logsError == null)
                return NotFound();

            _context.LogsErrors.Remove(logsError);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

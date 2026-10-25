using CulturalCenterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using CulturalCenterApi.Services;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CulturalCenterContext _context;
        private readonly UserService _userService;

        public EmployeesController(CulturalCenterContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployeesAsync()
        {
            var employeesList = await _context.Employees.ToListAsync();

            return Ok(employeesList);
        }

        [HttpGet("{idEmployee}")]
        public async Task<ActionResult<Employee>> GetEmployeeIdAsync(int idEmployee)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.IdEmployee == idEmployee);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployeeAsync([FromBody] Employee employeeBody, [FromQuery] bool createUserQuery = false, [FromQuery] string? roleQuery = null) 
        {
            if (employeeBody == null)
                return BadRequest();

            await _context.Employees.AddAsync(employeeBody);

            await _context.SaveChangesAsync();

            if (createUserQuery)
            {
                await _userService.CreateUserAsync(employeeBody,roleQuery);
            }            

            return Ok(employeeBody);
        }

        [HttpPut("{idEmployee}")]
        public async Task<IActionResult> PutEmployeeAsync([FromBody] Employee employeeBody, int idEmployee)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.IdEmployee == idEmployee);

            if (employee == null)
                return NotFound();

            employee.FirstName = employeeBody.FirstName;
            employee.LastName = employeeBody.LastName;
            employee.MiddleName = employeeBody.MiddleName;
            employee.NumberPhone = employeeBody.NumberPhone;
            employee.DateBirth = employeeBody.DateBirth;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpDelete("{idEmployee}")]
        public async Task<ActionResult<Employee>> DeleteEmployeeAsync(int idEmployee)
        {
            var employee = await _context.Employees.Where(e => e.IdEmployee == idEmployee).FirstOrDefaultAsync();

            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

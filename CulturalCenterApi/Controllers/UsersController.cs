using CulturalCenterApi.Models;
using CulturalCenterApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CulturalCenterContext _context;
        private readonly UserService _userService;

        public UsersController (CulturalCenterContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetUserAsync()
        {
            var usersList = await _context.Users.Select(u => new UserResponse
            {
                IdUser = u.IdUser,
                Login = u.Login,
                Role = u.Role,
                IdEmployee = u.IdEmployee
            }).ToListAsync();

            return Ok(usersList);
        }

        [HttpGet("{idUser}")]
        public async Task<ActionResult<UserResponse>> GetUserIdAsync (int idUser)
        {
            var user = await _context.Users.Where(u => u.IdUser == idUser).Select(u => new UserResponse
            {
                IdUser = u.IdUser,
                Login = u.Login,
                Role = u.Role,
                IdEmployee = u.IdEmployee
            }).FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("login/{login}/exclude/{idUser}")]
        public async Task<ActionResult<bool>> GetUserCheckLoginAsync(string login, int idUser)
        {
            bool exists = await _context.Users.AnyAsync(u => u.Login == login && u.IdUser != idUser);

            return Ok(exists);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> PostUserAsync([FromBody] User userBody)
        {
            if (userBody == null)
                return BadRequest();

            await _userService.CreateUserAsync(user: userBody);

            return Ok(new UserResponse
            {
                IdUser = userBody.IdUser,
                Login = userBody.Login,
                Role = userBody.Role,
                IdEmployee = userBody.IdEmployee
            });
        }

        [HttpPost("check-password")]
        public async Task<IActionResult> CheckCorrectPasswordUserAsync([FromBody] LoginRequest loginRequestBody)
        {
            if (loginRequestBody == null)
                return BadRequest();

            var isCorrect = await _userService.CheckCorrectPasswordAsync(loginRequestBody);

            if (!isCorrect)
                return Unauthorized();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> LoginAsync([FromBody] LoginRequest loginRequestBody)
        {
            User? user = null;

            if (loginRequestBody == null)
                return BadRequest();

            if (!await _userService.CheckCorrectPasswordAsync(loginRequestBody))
                return Unauthorized();

            user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginRequestBody.Login);

            if (user == null)
                return NotFound();

            return Ok(new UserResponse
            {
                IdUser = user.IdUser,
                Login = user.Login, 
                Role = user.Role,
                IdEmployee = user.IdEmployee
            });
        }

        [HttpPut("{idUser}")]
        public async Task<ActionResult<UserResponse>> PutUserAsync(int idUser, [FromBody] User userBody)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return NotFound();

            if (userBody == null)
                return BadRequest();

            user.Login = userBody.Login;
            user.Role = userBody.Role;
            user.IdEmployee = userBody.IdEmployee;

            await _context.SaveChangesAsync();

            return Ok(new UserResponse
            {
                IdUser = user.IdUser, 
                Login = user.Login,
                Role = user.Role,
                IdEmployee = user.IdEmployee
            });
        }

        [HttpPut("reset-password/{idUser}")]
        public async Task<IActionResult> ResetPasswordAsync(int idUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return NotFound();

            user.Password = await _userService.HashPasswordAsync("1234");

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("account")]
        public async Task<IActionResult> AccountChangeModel([FromBody] AccountChangeModel accountChangeModelBody)
        {
            if (accountChangeModelBody == null)
                return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == accountChangeModelBody.IdUser);

            if (user == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(accountChangeModelBody.NewPassword))
                user.Password = await _userService.HashPasswordAsync(accountChangeModelBody.NewPassword);

            user.Login = accountChangeModelBody.Login;
            user.Role = accountChangeModelBody.Role;
            user.IdEmployee = accountChangeModelBody.IdEmployee;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{idUser}")]
        public async Task<IActionResult> DeleteUserAsync(int idUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

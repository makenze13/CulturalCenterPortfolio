using CulturalCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CulturalCenterApi.Services
{
    public class UserService
    {
        private readonly CulturalCenterContext _context;

        public UserService(CulturalCenterContext context)
        {
            _context = context;
        }

        public async Task CreateUserAsync(Employee employee = null, string role = null, User user = null)
        {
            string hashPassword;
            string password;
            User newUser = user == null ? new User() : user;

            password = user != null ? user.Password : password = "1234";

            hashPassword = await HashPasswordAsync(password);

            if (employee != null)
            {
                newUser.Login = employee.NumberPhone;
                newUser.Role = role;
                newUser.IdEmployee = employee.IdEmployee;
            }

            newUser.Password = hashPassword;

            await _context.Users.AddAsync(newUser);

            await _context.SaveChangesAsync();
        }

        public async Task<String> HashPasswordAsync (string password)
        {
            var hashPassword = await _context.Database.SqlQuery<string>($"""SELECT crypt({password}, gen_salt('bf')) AS "Value" """).SingleAsync();

            return hashPassword;
        }

        public async Task<bool> CheckCorrectPasswordAsync(LoginRequest loginRequest)
        {  
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == loginRequest.Login);

            if (user != null)
            {
                var isCorrectPassword = await _context.Database.SqlQuery<bool>($"""SELECT crypt({loginRequest.Password}, {user.Password}) = {user.Password} AS "Value" """).SingleAsync();

                if (isCorrectPassword)
                    return true;
            }

            return false;
        }
    }
}

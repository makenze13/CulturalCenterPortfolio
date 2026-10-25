using CulturalCenterWeb.Models;
using CulturalCenterWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace CulturalCenterWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserApiService _userApiService;
        private readonly TicketApiService _ticketApiService;
        private readonly EventApiService _eventApiService;

        public AccountController(UserApiService userApiService, TicketApiService ticketApiService, EventApiService eventApiService)
        {
            _userApiService = userApiService;
            _ticketApiService = ticketApiService;
            _eventApiService = eventApiService;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ticketsList = await _ticketApiService.GetUserTicketsAsync(idUser);

            List<TicketEventGeneralInfoModel> ticketFullInfoList = new();

            foreach (var ticket in ticketsList)
            {
                var eventInfo = await _eventApiService.GetEventIdAsync(ticket.IdEvent);

                TicketEventGeneralInfoModel ticketFullInfo = new()
                {
                    IdTicket = (int)ticket.IdTicket,
                    IdEvent = ticket.IdEvent,
                    EventDate = eventInfo.EventDate,
                    EventName = eventInfo.EventName,
                    Seat = ticket.Seat,
                    TicketPrice = ticket.TicketPrice,
                    TypeEvent = eventInfo.TypeEvent,
                    UnicTicketNumber = ticket.UnicTicketNumber
                };

                ticketFullInfoList.Add(ticketFullInfo);
            }

            return View(ticketFullInfoList);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = await _userApiService.LoginAsync(login, password);

            if (user == null || user.Role != "Клиент")
            {
                ModelState.AddModelError("", "Неверный логин или пароль!");
                return View("Login");
            }

            await CreateClaim(user.IdUser, user.Login);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string login, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                ModelState.AddModelError(nameof(confirmPassword), "Пароли не совпадают!");
            else if (await _userApiService.GetUserCheckLogin(login, 0))
                ModelState.AddModelError(nameof(login), "Пользователь с таким логином уже существует");

            if (!ModelState.IsValid)
            {
                ViewBag.ShowRegister = true;
                return View("Login");
            }

            UserModel userModel = new()
            {
                Login = login,
                Password = password,
                Role = "Клиент"
            };

            var user = await _userApiService.PostUserAsync(userModel);

            if (user == null)
            {
                ModelState.AddModelError("", "Не удалось создать пользователя!");
                return View("Login");
            }

            await CreateClaim(user.IdUser, user.Login);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Login", "Account");
        }

        private async Task CreateClaim(int id, string login)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, id.ToString()),
                new(ClaimTypes.Name, login)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);
        }
    }
}

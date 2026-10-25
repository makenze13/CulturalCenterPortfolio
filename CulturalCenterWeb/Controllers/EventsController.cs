using CulturalCenterWeb.Models;
using CulturalCenterWeb.Services;
using CulturalCenterWeb.ViewsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Transactions;

namespace CulturalCenterWeb.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventApiService _eventApiService;
        private readonly TicketApiService _ticketApiService;
        private SeatSectorsViewModel seatSectors;

        public EventsController(EventApiService eventApiService, TicketApiService ticketApiService)
        {
            _eventApiService = eventApiService;
            _ticketApiService = ticketApiService;
        }

        public async Task<IActionResult> Index(int page = 1, string? typeEvent = null)
        {
            var events = await _eventApiService.GetEventsWithParameters(typeEvent);

            var pageEventSize = 18;
            int totalPages = (int)Math.Ceiling(events.Count / (double)pageEventSize);

            page = Math.Clamp(page, 1, Math.Max(totalPages, 1));

            var pagesEvents = events.Skip((page - 1) * pageEventSize).Take(pageEventSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.CurrentTypeEvent = typeEvent;
            ViewBag.TotalPages = totalPages;
            
            return View(pagesEvents);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _eventApiService.GetEventIdAsync(id);

            return View(item);
        }

        public async Task<IActionResult> SelectSeats(int id)
        {
            ViewBag.ChooseSeat = false;

            var seatSectors = await ReturnSeatSectors(id);

            return View(seatSectors);
        }

        [HttpPost]
        public async Task<IActionResult> SelectSeats(int id, string row, string number, string sector, decimal price)
        {
            ViewBag.ChooseSeat = true;
            ViewBag.Row = row;
            ViewBag.Number = number;
            ViewBag.Sector = sector;
            ViewBag.Price = price;

            var seatSectors = await ReturnSeatSectors(id);

            return View(seatSectors);
        }

        private async Task<SeatSectorsViewModel> ReturnSeatSectors(int id)
        {
            var eventDto = await _eventApiService.GetEventIdAsync(id);

            var seatSectors = new SeatSectorsViewModel
            {
                EventDto = eventDto,
                SectorC1 = await _eventApiService.CreateSectorAsync(id, "C1", eventDto),
                SectorC2 = await _eventApiService.CreateSectorAsync(id, "C2", eventDto),
                SectorC3 = await _eventApiService.CreateSectorAsync(id, "C3", eventDto)
            };

            return seatSectors;
        }

        [Authorize]
        public async Task<IActionResult> Payment(int idEvent, string sector, string row, string number)
        {
            var eventDto = await _eventApiService.GetEventIdAsync(idEvent);
            var seatPrice = (await _eventApiService.CreateSectorAsync(idEvent, sector, eventDto)).FirstOrDefault().Price;

            Seat seat = new()
            {
                SeatEventDTO = eventDto,
                Sector = sector,
                Row = int.Parse(row),
                Number = int.Parse(number), 
                Price = seatPrice
            };

            ViewBag.Seat = seat;

            return View(seat);
        }

        public async Task<IActionResult> PaymentSuccess(int idEvent, string? sector, string? row, string? number, int? idTicket = null)
        {
            var eventDto = await _eventApiService.GetEventIdAsync(idEvent);
            Ticket viewTicket;

            if (idTicket == null)
            {
                var seatPrice = (await _eventApiService.CreateSectorAsync(idEvent, sector, eventDto)).FirstOrDefault().Price;

                Ticket ticket = new Ticket
                {
                    UnicTicketNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                    IdEvent = idEvent,
                    IdUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Seat = sector + "-" + row + "-" + number,
                    TicketPrice = seatPrice,
                    SellDateTime = DateTime.Now
                };

                viewTicket = await _ticketApiService.PostTicketAsync(ticket);

                if (viewTicket == null)
                    return BadRequest();
            }
            else
            {
                viewTicket = await _ticketApiService.GetIdTicketAsync((int)idTicket);
            }

            ViewBag.EventDto = eventDto;

            return View(viewTicket);
        }
    }
}

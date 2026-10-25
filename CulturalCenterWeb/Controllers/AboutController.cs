using Microsoft.AspNetCore.Mvc;

namespace CulturalCenterWeb.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

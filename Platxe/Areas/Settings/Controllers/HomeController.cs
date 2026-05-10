using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Platxe.Areas.Settings.Controllers
{
    [Area("Settings")]
	[Authorize(Roles = "PL-Settings")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

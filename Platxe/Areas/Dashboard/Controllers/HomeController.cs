using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
	[Authorize(Roles = "OMEs-Dashboard")]
	[Authorize(Roles = "PL-Dashboard")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

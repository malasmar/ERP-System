using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
	[Authorize(Roles = "OMEs-Auditing")]
	[Authorize(Roles = "PL-Auditing")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

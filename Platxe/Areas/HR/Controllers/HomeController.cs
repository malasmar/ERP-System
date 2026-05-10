using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.HR.Controllers
{
    [Area("HR")]
	[Authorize(Roles = "OMEs-HR")]
	[Authorize(Roles = "PL-HR")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

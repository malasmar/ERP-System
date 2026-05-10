using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Platxe.Areas.Manager.Controllers
{
    [Area("Manager")]
	[Authorize(Roles = "PL-Admin")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

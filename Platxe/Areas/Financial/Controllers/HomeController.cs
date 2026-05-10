using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

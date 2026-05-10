using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Platxe.Areas.Accounting.Controllers
{
    [Area("Accounting")]
	[Authorize(Roles = "OMEs-Accounting")]
	[Authorize(Roles = "PL-Accounting")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

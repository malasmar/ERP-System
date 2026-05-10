using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.Payroll.Controllers
{
    [Area("Payroll")]
	[Authorize(Roles = "OMEs-Payroll")]
	[Authorize(Roles = "PL-Payroll")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    [Route("Payroll/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Payroll")]
	[Authorize(Roles = "PL-Payroll")]
	public class SalariesController : Controller
    {
        public IActionResult CreateSalaries()
        {
            return View();
        }
        public IActionResult UpdateAttendance()
        {
            return View();
        }
        public IActionResult UpdateBenefit()
        {
            return View();
        }
        public IActionResult UpdateDeduction()
        {
            return View();
        }
        public IActionResult SalariesSheet()
        {
            return View();
        }
    }
}

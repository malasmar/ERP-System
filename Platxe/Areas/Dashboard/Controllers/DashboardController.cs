using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Route("Dashboard/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Dashboard")]
	[Authorize(Roles = "PL-Dashboard")]
	public class DashboardController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public DashboardController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult Sales()
        {
            return View();
        }
        public IActionResult Accounting()
        {
            return View();
        }
        public IActionResult Purchase()
        {
            return View();
        }
        public IActionResult CostCenter()
        {
            return View();
        }
        public IActionResult Financial()
        {
            return View();
        }
    }
}

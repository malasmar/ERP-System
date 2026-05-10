using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Security.Claims;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class ReportsCostCenterController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsCostCenterController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult CostCenterBalance()
        {
            return View();
        }
        public IActionResult IncomeSummary(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_IncomeSummary");
        }
        public IActionResult CostStatment()
        {
            return View();
        }
    }
}

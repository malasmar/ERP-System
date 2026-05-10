using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class ReportsProjectController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsProjectController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult ProjectBalance()
        {
            return View();
        }
        public IActionResult IncomeSummary(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_IncomeSummary");
        }
        public IActionResult ProjectStatment()
        {
            return View();
        }
    }
}

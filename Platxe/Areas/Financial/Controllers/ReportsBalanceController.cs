using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class ReportsBalanceController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsBalanceController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult CurrentAccount()
        {
            return View();
        }
        public IActionResult Employee()
        {
            return View();
        }
        public IActionResult Cash()
        {
            return View();
        }
        public IActionResult Bank()
        {
            return View();
        }
        public IActionResult Expense()
        {
            return View();
        }
        public IActionResult Revenue()
        {
            return View();
        }

        public IActionResult AccountStatment(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_AccountStatment");
        }
     
    }
}

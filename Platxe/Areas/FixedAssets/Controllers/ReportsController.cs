using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.FixedAssets.Controllers
{
    [Area("FixedAssets")]
    [Route("FixedAssets/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-FixedAssets")]
	[Authorize(Roles = "PL-FixedAssets")]
	public class ReportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public ReportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult FixureStatment()
        {
            return View();
        }
        public IActionResult FixtureBalance()
        {
            return View();
        }
        public IActionResult AmortizationBalance()
        {
            return View();
        }
        public IActionResult DetailsSheet()
        {
            return View();
        }
        public IActionResult SummarySheet()
        {
            return View();
        }
    }
}

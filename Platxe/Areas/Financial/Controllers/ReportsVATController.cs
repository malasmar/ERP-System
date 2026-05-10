using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class ReportsVATController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public ReportsVATController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult Declaration()
        {
            return View();
        }
        public IActionResult vatStatment()
        {
            return View();
        }
        public IActionResult vatStatmentDetails()
        {
            return View();
        }
        public IActionResult DeclarationSheet()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
    [Authorize(Roles = "OMEs-Financial")]
    [Authorize(Roles = "PL-Financial")]
    public class ReportsAdvanceController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsAdvanceController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult TransactionsDetails()
        {
            return View();
        }
    }
}

using CLiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
    [Authorize(Roles = "OMEs-Financial")]
    [Authorize(Roles = "PL-Financial")]
    public class OperationController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        private readonly Guid UserKey;
        public OperationController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }

        public IActionResult PostSalaries()
        {
            return View();
        }
        public IActionResult PostSalariesPartial()
        {
            return PartialView("_PostSalaries", new CLiFinancial.Documents.Transaction().GetItem(DB, xLan, UserKey, null, (int)DocumentKind.finJournalVoucher, Year));
        }
    }
}

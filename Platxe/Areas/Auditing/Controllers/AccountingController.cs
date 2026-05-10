using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
    [Route("Auditing/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Auditing")]
	[Authorize(Roles = "PL-Auditing")]
	public class AccountingController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public AccountingController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult GeneralLedgerDiff()
        {
            return View();
        }
        public IActionResult ParentTransactions()
        {
            return View();
        }
        public IActionResult NullGeneralLedger()
        {
            return View();
        }
        public IActionResult TransferTransactions()
        {
            return View();
        }
        public IActionResult CloseFinancialYear()
        {
            return View();
        }
    }
}

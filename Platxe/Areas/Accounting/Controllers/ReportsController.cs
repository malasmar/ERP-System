using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    [Route("Accounting/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Accounting")]
	[Authorize(Roles = "PL-Accounting")]
	public class ReportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public ReportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult TrialBalance()
        {
            return View();
        }
        public IActionResult TrialBalanceTree()
        {
            return View();
        }
        public IActionResult TrialBalanceTL()
        {
            return View();
        }
        public IActionResult IncomeStatement()
        {
            return View();
        }
        public IActionResult BalanceStatement()
        {
            return View();
        }
        public IActionResult AccountStatement()
        {
            return View();
        }
        public IActionResult ParentStatement()
        {
            return View();
        }
        public IActionResult InteractiveTrial()
        {
            return View();
        }
        public IActionResult IncomeStatementv2()
        {
            return View();
        }
        public IActionResult xInteractiveTrial(string Code)
        {
            ViewBag.Code = Code;
            return PartialView("_InteractiveTrial");
        }
        public IActionResult YearlyStatement()
        {
            return View();
        }
        public IActionResult TreeBalance()
        {
            return View();
        }
    }
}

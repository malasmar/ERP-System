using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
	public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult CurrentAccount()
        {
            return View();
        }
        public IActionResult CurrentAccountDetails(Guid? Key)
        {
            return PartialView("_CurrentAccount", new CLiFinancial.Cards.CurrentAccount().GetItem(DB, Key));
        }

        public IActionResult Bank()
        {
            return View();
        }
        public IActionResult BankDetails(Guid? Key)
        {
            return PartialView("_Bank", new CLiFinancial.Cards.Bank().GetItem(DB, Key));
        }

        public IActionResult CashBox()
        {
            return View();
        }
        public IActionResult CashBoxDetails(Guid? Key)
        {
            return PartialView("_CashBox", new CLiFinancial.Cards.CashBox().GetItem(DB, Key));
        }

        public IActionResult Expenses()
        {
            return View();
        }
        public IActionResult ExpensesDetails(Guid? Key)
        {
            return PartialView("_Expenses", new CLiFinancial.Cards.Expenses().GetItem(DB, Key));
        }

        public IActionResult Revenue()
        {
            return View();
        }
        public IActionResult RevenueDetails(Guid? Key)
        {
            return PartialView("_Revenue", new CLiFinancial.Cards.Revenue().GetItem(DB, Key));
        }
    }
}

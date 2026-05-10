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
	public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult Roots()
        {
            return View();
        }
        public IActionResult GetRoot(Guid? Key)
        {
            return PartialView("_root", new CLiAccounting.Cards.Root().GetItem(DB, Key));
        }
        public IActionResult Accounts()
        {
            return View();
        }
        public IActionResult GetAccount(Guid? Key)
        {
            return PartialView("_account", new CLiAccounting.Cards.Account().GetItem(DB, Key));
        }
        public IActionResult AccountsTree()
        {
            return View();
        }
        public IActionResult ChartofAccountTree()
        {
            return View();
        }
        public IActionResult BalanceSheetItems()
        {
            return View();
        }
        public IActionResult GetBalanceSheetItems(Guid? Key)
        {
            return PartialView("_BalanceSheetItems", new CLiAccounting.Cards.BalanceSheetItems().GetItem(DB, Key));
        }
        public IActionResult BalanceSheetItemsTree()
        {
            return View();
        }
        public IActionResult CostCenter()
        {
            return View();
        }
        public IActionResult CostCenterDetails(Guid? Key)
        {
            return PartialView("_CostCenter", new CLiAccounting.Cards.CostCenter().GetItem(DB, Key));
        }
        public IActionResult Project()
        {
            return View();
        }
        public IActionResult ProjectDetails(Guid? Key)
        {
            return PartialView("_Project", new CLiAccounting.Cards.Project().GetItem(DB, Key));
        }
        public IActionResult BulkEdit()
        {
            return View();
        }
    }
}

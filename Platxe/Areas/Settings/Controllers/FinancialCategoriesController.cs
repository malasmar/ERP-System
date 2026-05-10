using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Route("Settings/[Controller]/[Action]")]
	[Authorize(Roles = "PL-Settings")]
	public class FinancialCategoriesController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public FinancialCategoriesController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult GrpCurrentAccount()
        {
            return View();
        }
        public IActionResult GrpCurrentAccountDetails(Guid? Key)
        {
            return PartialView("_GrpCurrentAccount", new CLiFinancial.Cards.Groups.CurrentAccountGroup().GetItem(DB, Key));
        }
        public IActionResult GrpActivity()
        {
            return View();
        }
        public IActionResult GrpActivityDetails(Guid? Key)
        {
            return PartialView("_GrpActivity", new CLiFinancial.Cards.Groups.Activity().GetItem(DB, Key));
        }

        public IActionResult GrpCashBox()
        {
            return View();
        }
        public IActionResult GrpCashBoxDetails(Guid? Key)
        {
            return PartialView("_GrpCashBox", new CLiFinancial.Cards.Groups.CashboxGroup().GetItem(DB, Key));
        }

        public IActionResult GrpExpenses()
        {
            return View();
        }
        public IActionResult GrpExpensesDetails(Guid? Key)
        {
            return PartialView("_GrpExpenses", new CLiFinancial.Cards.Groups.ExpensesGroup().GetItem(DB, Key));
        }

        public IActionResult GrpRevenue()
        {
            return View();
        }
        public IActionResult GrpRevenueDetails(Guid? Key)
        {
            return PartialView("_GrpRevenue", new CLiFinancial.Cards.Groups.RevenueGroup().GetItem(DB, Key));
        }


        public IActionResult TransactionCategories()
        {
            return View();
        }
        public IActionResult TransactionCategoriesDetails(Guid? Key)
        {
            return PartialView("_TransactionCategories", new CLiFinancial.Cards.Groups.TransactionCategories().GetItem(DB, Key));
        }

        public IActionResult JVCategories()
        {
            return View();
        }
        public IActionResult JVCategoriesDetails(Guid? Key)
        {
            return PartialView("_JVCategories", new CLiFinancial.Cards.Groups.JVCategories().GetItem(DB, Key));
        }
        public IActionResult SalariesIntegration()
        {
            return View();
        }
        public IActionResult SalariesIntegrationDetails(Guid? Key)
        {
            return PartialView("_SalariesIntegration", new CLiFinancial.Cards.SalariesIntegration().GetItem(DB, Key));
        }

    }
}

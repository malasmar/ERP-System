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
	public class ConfigurationController : Controller
    {
        private readonly IWebHostEnvironment root;
        private readonly string DB;
        private readonly int UserID;
        public ConfigurationController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult CompanyProfile()
        {
            return View(new CLiCore.Configuration.Profile().GetItem(DB));
        }
        public IActionResult DefaultSettings()
        {
            return View(new CLiCore.Configuration.DefaultSettings().GetItem(DB));
        }
        public IActionResult vatRate()
        {
            return View();
        }
        public IActionResult vatRateDetails(Guid? Key)
        {
            return PartialView("_vatRate", new CLiCore.Configuration.vatRates().GetItem(DB, Key));
        }
        public IActionResult vatKind()
        {
            return View();
        }
        public IActionResult vatKindDetails(Guid? Key)
        {
            return PartialView("_vatKind", new CLiCore.Configuration.vatKind().GetItem(DB, Key));
        }
        public IActionResult Branch()
        {
            return View();
        }
        public IActionResult BranchDetails(Guid? Key)
        {
            return PartialView("_Branch", new CLiCore.Configuration.Branch().GetItem(DB, Key));
        }

        public IActionResult Prefix()
        {
            return View();
        }
        public IActionResult PrefixDetails(Guid? Key)
        {
            return PartialView("_Prefix", new CLiCore.Configuration.Prefix().GetItem(DB, Key));
        }

        public IActionResult Currency()
        {
            return View();
        }
        public IActionResult CurrencyDetails(Guid? Key)
        {
            return PartialView("_Currency", new CLiCore.Configuration.Currency().GetItem(DB, Key));
        }

        public IActionResult FinancialYear()
        {
            return View();
        }
        public IActionResult FinancialYearDetails(Guid? Key)
        {
            return PartialView("_FinancialYear", new CLiCore.Configuration.FinancialYear().GetItem(DB, Key));
        }
        public IActionResult Cities()
        {
            return View();
        }
        public IActionResult CityDetails(Guid? Key)
        {
            return PartialView("_City", new CLiCore.Configuration.City().GetItem(DB, Key));
        }
        public IActionResult Settings()
        {
            return View(new CLiCore.Configuration.Settings().GetItem(DB));
        }
        public IActionResult ClientCategories()
        {
            return View();
        }
        public IActionResult ClientCategoriesDetails(Guid? Key)
        {
            return PartialView("_ClientCategory", new CLiCore.Configuration.ClientCategories().GetItem(DB, Key));
        }
        public IActionResult EmployeeAccounts()
        {
            return PartialView("_EmployeeAccounts", new CLiCore.Configuration.EmployeeAccounts().GetItem(DB));
        }
    }
}

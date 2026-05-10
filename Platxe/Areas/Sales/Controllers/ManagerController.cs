using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Route("Sales/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Sales")]
	[Authorize(Roles = "PL-Sales")]
	public class ManagerController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ManagerController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }
        public IActionResult SalesInvoices()
        {
            return View();
        }
        public IActionResult ReturnInvoices()
        {
            return View();
        }
        public IActionResult Quotations()
        {
            return View();
        }
        public IActionResult Proforma()
        {
            return View();
        }
        public IActionResult Contracts()
        {
            return View();
        }

        public IActionResult AccountStatment()
        {
            return View();
        }
        public IActionResult AccountBalance()
        {
            return View();
        }
    }
}

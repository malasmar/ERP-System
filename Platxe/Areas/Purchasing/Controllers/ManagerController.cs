using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Purchasing")]
	[Authorize(Roles = "PL-Purchasing")]
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
        public IActionResult PurchaseInvoices()
        {
            return View();
        }
        public IActionResult ReturnInvoices()
        {
            return View();
        }
    }
}

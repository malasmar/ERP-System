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
    public class ReportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult TransactionsDetails()
        {
            return View();
        }
        public IActionResult ReturnDetails()
        {
            return View();
        }
    }
}

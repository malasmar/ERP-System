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
	public class ReportsStatmentController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public ReportsStatmentController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult AccountStatment(int CurrentKind)
        {
            ViewBag.CurrentKind = CurrentKind;
            return View();
        }
        public IActionResult SummaryStatment(int CurrentKind)
        {
            ViewBag.CurrentKind = CurrentKind;
            return View();
        }
        public IActionResult EmployeeStatment()
        {
            return View();
        }
    }
}

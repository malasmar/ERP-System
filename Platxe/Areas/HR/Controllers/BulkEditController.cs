using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Platxe.Areas.HR.Controllers
{
    [Area("HR")]
    [Route("HR/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-HR")]
	[Authorize(Roles = "PL-HR")]
	public class BulkEditController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public BulkEditController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult General()
        {
            return View();
        }
        public IActionResult Salaries()
        {
            return View();
        }
        public IActionResult Insurance()
        {
            return View();
        }
        public IActionResult Contract()
        {
            return View();
        }
        public IActionResult Attendance()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}

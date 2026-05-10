using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;

namespace Platxe.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/[Controller]/[Action]")]
    [Authorize(Roles = "PL-Admin")]
    public class AdminController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly Guid Subscribe;
        public AdminController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
        }

        public IActionResult Whatsapp()
        {
            return View(new CLiCore.Platx.Whatsapp().GetItem(DB));
        }
        public IActionResult Email()
        {
            return View(new CLiCore.Configuration.MyEmail().GetItem(DB));
        }
    }
}

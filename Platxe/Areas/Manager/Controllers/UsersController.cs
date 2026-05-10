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
	public class UsersController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly Guid Subscribe;
        public UsersController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
        }

        public IActionResult Users()
        {
            return View();
        }
        public IActionResult UserDetails(Guid? Key)
        {
            return PartialView("_User", new CLiCore.Platx.Users().GetItem(Key));
        }


        public IActionResult Technical()
        {
            return View();
        }
        public IActionResult TechnicalDetails(Guid? Key)
        {
            return PartialView("_Technical", new CLiCore.Platx.Technical().GetItem(Key, Subscribe));
        }
        public IActionResult AppUsers()
        {
            return View();
        }
        public IActionResult PaymentMethodDetails(Guid? Key,Guid? User)
        {
            return PartialView("_PaymentMethod", new CLiCore.Platx.PaymentMethods().GetItem(DB, Key, User));
        }
    
    }
}

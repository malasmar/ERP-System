using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly Guid? UserKey;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }
        public async Task<IActionResult> Index()
        {
            bool dis = CLiCore.Account.core.CheckUserIfDisable(UserKey);
            if (dis == true)
            {
                await HttpContext.SignOutAsync();
               return RedirectToAction("Login", "Account");
                
            }
           

            return View(new CLiCore.Configuration.Profile().GetItem(DB));
        }

		//[Route("error")]
		//public IActionResult Error() {
  //          return View();
  //      }
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}

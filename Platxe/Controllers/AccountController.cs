using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Platxe.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword");
        }
        public IActionResult RecoverPassword()
        {
            return View();
        }
        public IActionResult SuccessfulSendPassword()
        {
            return View();
        }
        public IActionResult OTP(Guid? Key)
        {
            if (Key == null)
            {
              return  RedirectToAction("error404", "errors");
            }
            Guid k;
            if (Guid.TryParse(Key.ToString(),out k) == false)
            {
				return RedirectToAction("error404", "errors");
			}
            ViewBag.Key = Key;
            return View();
        }
    }
}

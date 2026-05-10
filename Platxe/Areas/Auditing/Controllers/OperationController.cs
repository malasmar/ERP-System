using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
    [Route("Auditing/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Auditing")]
	[Authorize(Roles = "PL-Auditing")]
	public class OperationController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public OperationController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult UnpostedVouchers()
        {
            return View();
        }
        public IActionResult PostVAT()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Platxe.Areas.HR.Controllers
{
    [Area("HR")]
    [Route("HR/[Controller]/[Action]")]
    [Authorize(Roles = "OMEs-HR")]
    [Authorize(Roles = "PL-HR")]
    public class RequestController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public RequestController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Advance()
        {
            return View();
        }
    }
}

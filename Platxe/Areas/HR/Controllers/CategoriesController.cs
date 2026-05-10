using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.HR.Controllers
{
    [Area("HR")]
    [Route("HR/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-HR")]
	[Authorize(Roles = "PL-HR")]
	public class CategoriesController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CategoriesController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Reward()
        {
            return View();
        }
        public IActionResult RewardDetails(Guid? Key)
        {
            return PartialView("_Reward", new CLiHR.Categories.Reward().GetItem(DB, Key));
        }
        public IActionResult Penalty()
        {
            return View();
        }
        public IActionResult PenaltyDetails(Guid? Key)
        {
            return PartialView("_Penalty", new CLiHR.Categories.Penalty().GetItem(DB, Key));
        }
        public IActionResult CatRequest()
        {
            return View();
        }
        public IActionResult CatRequestDetails(Guid? Key)
        {
            return PartialView("_CatRequest", new CLiHR.Categories.Request().GetItem(DB, Key));
        }
    
    }
}

using CLiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.FixedAssets.Controllers
{
    [Area("FixedAssets")]
    [Route("FixedAssets/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-FixedAssets")]
	[Authorize(Roles = "PL-FixedAssets")]
	public class OperationsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public OperationsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult BookValues()
        {
            return View();
        }
        public IActionResult BookValuesDetails(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return PartialView("_BookValuesDetails", new CLiFinancial.FixedAssets.Operation.BookValues().GetItem(DB,UserID, Key));
        }
    }
}

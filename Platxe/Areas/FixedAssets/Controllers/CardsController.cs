using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.FixedAssets.Controllers
{
    [Area("FixedAssets")]
    [Route("FixedAssets/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-FixedAssets")]
	[Authorize(Roles = "PL-FixedAssets")]
	public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Categories()
        {
            return View();
        }
        public IActionResult CategoriesDetails(Guid? Key)
        {
            return PartialView("_Categories", new CLiFinancial.FixedAssets.Cards.Categories().GetItem(DB,Key));
        }

        public IActionResult Fixture()
        {
            return View();
        }
        public IActionResult FixtureDetails(Guid? Key)
        {
            return PartialView("_Fixture", new CLiFinancial.FixedAssets.Cards.Fixture().GetItem(DB, Key));
        }
    }
}

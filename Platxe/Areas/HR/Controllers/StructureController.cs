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
	public class StructureController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public StructureController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrganizationalStr()
        {
            return View();
        }
        public IActionResult StructureDetails(Guid? Key)
        {
            CLiHR.Cards.Structure item = new CLiHR.Cards.Structure().GetItem(DB, Key);
            if(item.Key !=null && item.Level == 1)
            {
                return PartialView("_Boss", item);
            }
            else
            {
                return PartialView("_Structure", item);
            }
       
        }
        public IActionResult BossDetails(Guid? Key)
        {
            return PartialView("_Boss", new CLiHR.Cards.Structure().GetItem(DB, Key));
        }
    }
}

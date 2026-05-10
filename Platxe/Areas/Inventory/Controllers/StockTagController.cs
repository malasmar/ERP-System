using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Inventory")]
	[Authorize(Roles = "PL-Inventory")]
	public class StockTagController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public StockTagController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult Color()
        {
            return View();
        }
        public IActionResult ColorDetails(Guid? Key)
        {
            return PartialView("_Color", new CLiInventory.Cards.Tag.Color().GetItem(DB, Key));
        }
        public IActionResult Size()
        {
            return View();
        }
        public IActionResult SizeDetails(Guid? Key)
        {
            return PartialView("_Size", new CLiInventory.Cards.Tag.Size().GetItem(DB, Key));
        }
        public IActionResult Price()
        {
            return View();
        }
        public IActionResult PriceDetails(Guid? Key)
        {
            return PartialView("_Price", new CLiInventory.Cards.Tag.Price().GetItem(DB, Key));
        }
    }
}

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
	public class StockBatchesController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public StockBatchesController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult ItemBatch()
        {
            return View();
        }
        public IActionResult ItemBatches(Guid? Key)
        {
            return PartialView("_ItemBatches", new CLiInventory.JsonData.jnStockItem().GetItem(DB, Key));
        }
        public IActionResult BatchDetails(Guid? Key, Guid? Item)
        {
            return PartialView("_ItemBatch", new CLiInventory.Cards.Stock.ItemBatch().GetItem(DB, Key, Item));
        }
    }
}

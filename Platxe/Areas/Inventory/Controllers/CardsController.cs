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
	public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }

        public IActionResult Warehouse()
        {
            return View();
        }
        public IActionResult WarehouseDetails(Guid? Key)
        {
            return PartialView("_Warehouse", new CLiInventory.Cards.Warehouse().GetItem(DB, Key));
        }

        public IActionResult Unit()
        {
            return View();
        }
        public IActionResult UnitDetails(Guid? Key)
        {
            return PartialView("_Unit", new CLiInventory.Cards.Unit().GetItem(DB, Key));
        }

        public IActionResult Category()
        {
            return View();
        }
        public IActionResult CategoryDetails(Guid? Key)
        {
            return PartialView("_Category", new CLiInventory.Cards.Category().GetItem(DB, Key));
        }
        public IActionResult StockItem()
        {
            return View();
        }
        public IActionResult PackageItems()
        {
            return View();
        }
        public IActionResult StockItemDetails(Guid? Key)
        {
            return PartialView("_StockItem", new CLiInventory.Cards.StockItem().GetItem(DB, Key));
        }
        public IActionResult StockUnits(Guid? Key)
        {
            return PartialView("_StockUnits", new CLiInventory.JsonData.jnStockItem().GetItem(DB, Key));
        }
        public IActionResult StockUnit(Guid? Key,Guid? Item)
        {
            return PartialView("_StockUnit", new CLiInventory.Cards.Stock.StockUnit().GetItem(DB, Key, Item));
        }
    }
}

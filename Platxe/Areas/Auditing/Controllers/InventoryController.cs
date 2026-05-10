using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
    [Route("Auditing/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Auditing")]
	[Authorize(Roles = "PL-Auditing")]
	public class InventoryController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public InventoryController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }
        public IActionResult StocktakingOffset()
        {
            return View();
        }
        public IActionResult StockOpeningBalance()
        {
            return View();
        }
        public IActionResult Stocktaking()
        {
            return PartialView("_Stocktaking");
        }



        public IActionResult StockStatment()
        {
            return View();
        }
        public IActionResult StockFullStatment()
        {
            return View();
        }
        public IActionResult WarehouseBalance()
        {
            return View();
        }
        public IActionResult WarehousesBalance()
        {
            return View();
        }
        public IActionResult BalanceBaseWarehouse()
        {
            return View();
        }
    }
}

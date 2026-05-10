using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Inventory")]
	[Authorize(Roles = "PL-Inventory")]
	public class ReportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public ReportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
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
        public IActionResult TransactionsDetails()
        {
            return View();
        }
    }
}

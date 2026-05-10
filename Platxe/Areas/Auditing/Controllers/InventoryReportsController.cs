using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
    [Route("Auditing/[Controller]/[Action]")]
    [Authorize(Roles = "OMEs-Auditing")]
    [Authorize(Roles = "PL-Auditing")]
    public class InventoryReportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public InventoryReportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult ItemsCostAudit()
        {
            return View();
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
        public IActionResult ItemsCostAuditWarehouse()
        {
            return View();
        }
        public IActionResult WarehousesValues()
        {
            return View();
        }
    }
}

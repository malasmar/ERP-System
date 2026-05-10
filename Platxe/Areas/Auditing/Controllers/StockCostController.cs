using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Auditing.Controllers
{
    [Area("Auditing")]
    [Route("Auditing/[Controller]/[Action]")]
    [Authorize(Roles = "OMEs-Auditing")]
    [Authorize(Roles = "PL-Auditing")]
    public class StockCostController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public StockCostController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }
        public IActionResult ItemsCost()
        {
            return View();
        }
        public IActionResult ItemCostDetails(Guid? Key)
        {
            return PartialView("_ItemCostDetails", new CLiInventory.JsonData.jnStockItem().GetItem(DB, Key));
        }
        public IActionResult ItemsCostPrices()
        {
            return View();
        }
        public IActionResult SalesInvoicesItemsCost()
        {
            return View();
        }
        public IActionResult RecalculateCost()
        {
            return View();
        }
     
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
    [Authorize]
    public class SelectionsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        public SelectionsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }
        public IActionResult Consumption()
        {
            return PartialView("_Consumption");
        }
        public IActionResult ConsumptionDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_ConsumptionDetails");
        }
        public IActionResult Shipping(int TargetWarehouse)
        {
            ViewBag.TargetWarehouse = TargetWarehouse;
            return PartialView("_Shipping");
        }
        public IActionResult ShippingDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_ShippingDetails");
        }
    
    }
}

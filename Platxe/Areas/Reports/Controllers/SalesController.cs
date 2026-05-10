using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Areas.Reports.Controllers
{
    [Area("Reports")]
    [Route("Reports/[Controller]/[Action]")]
    [Authorize(Roles = "PL-Reports")]
    public class SalesController : Controller
    {
        public IActionResult ItemsSales()
        {
            return View();
        }
        public IActionResult ItemsReturnSales()
        {
            return View();
        }
        public IActionResult ItemsNetSales()
        {
            return View();
        }
        public IActionResult MonthlyItemsNetSales()
        {
            return View();
        }
        public IActionResult MonthlyItemsQuantity()
        {
            return View();
        }
    }
}

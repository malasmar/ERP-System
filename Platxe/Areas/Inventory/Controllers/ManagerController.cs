using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Inventory")]
	[Authorize(Roles = "PL-Inventory")]
	public class ManagerController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public ManagerController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        public IActionResult OpeningStock()
        {
            return View();
        }
        public IActionResult ReConsumption()
        {
            return View();
        }
        public IActionResult Consumption()
        {
            return View();
        }
        public IActionResult TransferOut()
        {
            return View();
        }
        public IActionResult TransferIncome()
        {
            return View();
        }
    }
}

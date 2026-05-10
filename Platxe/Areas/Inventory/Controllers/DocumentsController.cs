using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Route("Inventory/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Inventory")]
	[Authorize(Roles = "PL-Inventory")]
	public class DocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid UserKey;
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }

        public IActionResult OpeningBalance(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB,xLan, UserKey, Key, (int)CLiCore.DocumentKind.invOpeningBalance));
        }

        public IActionResult Consumption(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB,xLan, UserKey, Key, (int)CLiCore.DocumentKind.ConsumptionStock));
        }

        public IActionResult ConsumptionReturn(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB,xLan, UserKey, Key, (int)CLiCore.DocumentKind.RetConsumptionStock));
        }

        public IActionResult SendToWarehouse(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.SendToWarehouse));
        }
        public IActionResult ReceiptInWarehouse(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.ReceiptInWarehouse));
        }
    }
}

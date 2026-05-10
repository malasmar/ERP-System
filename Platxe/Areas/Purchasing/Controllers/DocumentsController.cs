using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Purchasing")]
	[Authorize(Roles = "PL-Purchasing")]
	public class DocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        private readonly Guid UserKey;
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        public IActionResult PurchaseInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.PurchaseInvoice));
        }
        public IActionResult ReturnInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.ReturnPurchase));
        }

        public IActionResult xPurchaseInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.PurchaseInvoice));
        }

    }
}

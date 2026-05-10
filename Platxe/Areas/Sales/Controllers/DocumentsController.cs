using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Route("Sales/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Sales")]
	[Authorize(Roles = "PL-Sales")]
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
        public IActionResult SalesInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.SalesInvoice));
        }
        public IActionResult ReturnInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.ReturnSalesInvoice));
        }
        public IActionResult Quotation(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiSales.Documents.Quotation().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.Quotation, Year));
        }

        public IActionResult ProformaInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiSales.Documents.Proforma().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.SalesProformaInvoice, Year));
        }
        public IActionResult Contract(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiSales.Documents.Contract().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.SalesContract, Year));
        }

        public IActionResult xSalesInvoice(Guid? Key)
        {
            if (Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiInventory.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, (int)CLiCore.DocumentKind.SalesInvoice));
        }
    }
}

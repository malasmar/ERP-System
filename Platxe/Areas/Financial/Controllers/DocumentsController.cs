using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("Financial/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Financial")]
	[Authorize(Roles = "PL-Financial")]
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
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }

        public IActionResult Transaction(Guid? Key,int DocumentKind)
        {
            if (Key == null)
                ViewBag.Status = true;
            else
                ViewBag.Status = false;

            return View(new CLiFinancial.Documents.Transaction().GetItem(DB,xLan, UserKey, Key, DocumentKind, Year));
        }
        public IActionResult JournalVoucher(Guid? Key, int DocumentKind)
        {
            if (Key == null)
                ViewBag.Status = true;
            else
                ViewBag.Status = false;

            return View(new CLiFinancial.Documents.Transaction().GetItem(DB, xLan, UserKey, Key, DocumentKind, Year));
        }
        public IActionResult PostSalaries()
        {
            return View();
        }


    }
}

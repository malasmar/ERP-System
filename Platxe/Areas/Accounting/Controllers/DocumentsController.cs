using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Accounting.Controllers
{
    [Area("Accounting")]
    [Route("Accounting/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-Accounting")]
	[Authorize(Roles = "PL-Accounting")]
	public class DocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult GeneralLedger(Guid? Key)
        {
            if(Key == null)
                ViewBag.Status = false;
            else
                ViewBag.Status = true;

            return View(new CLiAccounting.Documents.GeneralLedger().GetItem(DB, Key));
        }
        public IActionResult GeneralLedgerList()
        {
            return View();
        }
    }
}

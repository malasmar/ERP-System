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
        public IActionResult AgesInvoices(int DocumentKind,Guid? Key)
        {
            ViewBag.DocumentKind = DocumentKind;
            ViewBag.Key = Key;
            return PartialView("_AgesInvoices");
        }
        public IActionResult SalariesPayment()
        {
            return PartialView("_SalariesPayment");
        }
    }
}

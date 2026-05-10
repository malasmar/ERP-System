using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
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
        public IActionResult SelectionInvoices()
        {
            return PartialView("_PurchaseInvoice");
        }
        public IActionResult InvoiceDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_InvoiceDetails");
        }
        public IActionResult AdvancePayments(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_AdvancePayments");
        }
        public IActionResult SearchItems(string Key)
        {
            ViewBag.Key = Key;
            return PartialView("_SearchItems");
        }
    }
}

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
            return PartialView("_SalesInvoices");
        }
        public IActionResult Quotations()
        {
            return PartialView("_Quotation");
        }
        public IActionResult Contracts()
        {
            return PartialView("_Contract");
        }
        public IActionResult Proforma()
        {
            return PartialView("_Proforma");
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
        public IActionResult QuotationDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_QuotationDetails");
        }
        public IActionResult ContractDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_ContractDetails");
        }
        public IActionResult ProformaDetails(Guid? Key)
        {
            ViewBag.Key = Key;
            return PartialView("_ProformaDetails");
        }
    }
}

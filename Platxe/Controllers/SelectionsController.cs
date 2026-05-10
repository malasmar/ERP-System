using CLiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Controllers
{
    [Authorize]
    public class SelectionsController : Controller
    {
     

        public IActionResult SalesInvoices()
        {
            return PartialView("_SalesInvoices");
        }
        public IActionResult CopyInvDocument(int DocumentKind)
        {
            ViewBag.DocumentKind = DocumentKind;
            return PartialView("_InvCopyDocument");
        }
        public IActionResult FinancialTransaction(int DocumentKind)
        {
            ViewBag.DocumentKind = DocumentKind;
            return PartialView("_FinancialTransaction");
        }
        public IActionResult JournalVoucher()
        {
            ViewBag.DocumentKind = (int)DocumentKind.finJournalVoucher;
            return PartialView("_JournalVoucher");
        }
    }
}

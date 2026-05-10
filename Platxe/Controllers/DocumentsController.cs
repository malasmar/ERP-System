using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public DocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        public IActionResult DocumentNote(Guid? Key)
        {
            return PartialView("_DocumentNote");
        }
        public IActionResult GeneralLedger(Guid? Key)
        {
            CLiAccounting.Documents.GeneralLedger transaction = new CLiAccounting.Documents.GeneralLedger().GetItem(DB, Key);
            return PartialView("GeneralLedger", transaction);
        }
        public IActionResult FinancialTransaction(Guid? Key)
        {
            CLiFinancial.Documents.TransactionInfo transaction = new CLiFinancial.Documents.TransactionInfo().GetItem(DB, xLan, Key);
            return PartialView("Transaction", transaction);
        }
        public IActionResult JournalVoucher(Guid? Key)
        {
            CLiFinancial.Documents.TransactionInfo transaction = new CLiFinancial.Documents.TransactionInfo().GetItem(DB, xLan, Key);
            return PartialView("JournalVoucher", transaction);
        }
        public IActionResult Quotation(Guid? Key)
        {
            CLiSales.Documents.QuotationInfo transaction = new CLiSales.Documents.QuotationInfo().GetItem(DB, xLan, Key);
            return PartialView("Quotation", transaction);
        }
        public IActionResult SalesInvoices(Guid? Key)
        {
            CLiInventory.Documents.TransactionInfo transaction = new CLiInventory.Documents.TransactionInfo().GetItem(DB, xLan, Key);
            return PartialView("SalesInvoices", transaction);
        }
        public IActionResult PurchaseInvoice(Guid? Key)
        {
            CLiInventory.Documents.TransactionInfo transaction = new CLiInventory.Documents.TransactionInfo().GetItem(DB, xLan, Key);
            return PartialView("PurchaseInvoice", transaction);
        }
        public IActionResult PrintTemplates(int DocKind, Guid? Key)
        {
            ViewBag.DocKind = DocKind;
            ViewBag.Key = Key;
            return PartialView("_PrintTemplates");
        }
        public IActionResult PrintTemplateSales(Guid? Key, int DocKind)
        {
            ViewBag.Key = Key;
            ViewBag.DocKind = DocKind;
            CLiCore.Print.PrintDetails details = new CLiCore.Print.PrintDetails().GetList(DB, Key);
            details.OperationKey = Key;
            return PartialView("_PrintTemplateSales", details);
        }
   public IActionResult PrintTemplateProforma(Guid? Key, int DocKind)
        {
            ViewBag.Key = Key;
            ViewBag.DocKind = DocKind;
            CLiCore.Print.PrintDetails details = new CLiCore.Print.PrintDetails().GetList(DB, Key);
            details.OperationKey = Key;
            return PartialView("_PrintTemplateProforma", details);
        }
    }
}

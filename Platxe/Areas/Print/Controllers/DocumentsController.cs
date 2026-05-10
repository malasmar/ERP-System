
using CLiCore;
using CLiCore.Print;
using FastReport;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Print.Controllers
{
    [Area("Print")]
    [Route("Print/[Controller]/[Action]")]
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IWebHostEnvironment root;
        private readonly string DB;
        private readonly int UserID;
        private readonly int Year;
        private readonly string xLan;
        public DocumentsController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PrintTransaction(Guid? Key, int DocKind, string Template)
        {

            CLiCore.Print.FinancialTransaction transaction = new CLiCore.Print.FinancialTransaction().GetItem(DB, Key);

            string webRootPath = root.WebRootPath;
            WebReport rep = new WebReport();
            rep.Report.Load(webRootPath + "/aiwa-data/" + DB + "/Template/Documents/" + Template);
            switch (DocKind)
            {
                case (int)DocumentKind.finJournalVoucher:
                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.Transaction(DB, Key), "Transaction Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.TransactionDetailsJV(DB, Key), "Transaction Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.BranchDetails(DB, transaction.Branch), "Branch Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
                    break;
                case (int)DocumentKind.ReturnSalesInvoice:
                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceHeader(DB, transaction.OperationKey), "Invoice Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceDetails(DB, transaction.OperationKey), "Invoice Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceQR(DB, transaction.OperationKey), "Invoice QR");
                    rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, transaction.AccountKey), "Current Account");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceHeader(DB, transaction.OriginalInvoice), "Original Invoice");
                    break;
                case (int)DocumentKind.PurchaseInvoice:
                case (int)DocumentKind.ReturnPurchase:

                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceHeader(DB, transaction.OperationKey), "Invoice Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceDetails(DB, transaction.OperationKey), "Invoice Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceQR(DB, transaction.OperationKey), "Invoice QR");
                    rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, transaction.AccountKey), "Current Account");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
                    break;
                case (int)DocumentKind.Quotation:
                    CLiCore.Print.FinancialTransaction Quotation = new CLiCore.Print.FinancialTransaction().GetQuotation(DB, Key);
                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.Quotation(DB, Quotation.OperationKey), "Invoice Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.QuotationDetails(DB, Quotation.OperationKey), "Invoice Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, Quotation.AccountKey), "Current Account");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(Quotation.CreateUser), "User Details");
                    break;
                case (int)DocumentKind.finCreditNote:
                case (int)DocumentKind.finDebitNote:
                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.Transaction(DB, Key), "Transaction Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.TransactionDetails(DB, Key), "Transaction Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.BranchDetails(DB, transaction.Branch), "Branch Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.TransactionQR(DB, transaction.OperationKey), "Transaction QR");

                    break;
                default:
                    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
                    rep.Report.RegisterData(CLiCore.Print.DDT.Transaction(DB, Key), "Transaction Header");
                    rep.Report.RegisterData(CLiCore.Print.DDT.TransactionDetails(DB, Key), "Transaction Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.BranchDetails(DB, transaction.Branch), "Branch Details");
                    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
                  
                    break;
            }



            rep.EmbedPictures = true;
            rep.ShowPdfExport = false;
            ViewBag.WebReport = rep;
            return View();
        }
        public IActionResult PrintSalesInvoice(CLiCore.Print.PrintDetails details)
        {

            CLiCore.Print.PrintDetails.Insert(DB, details);

            CLiCore.Print.FinancialTransaction transaction = new CLiCore.Print.FinancialTransaction().GetItem(DB, details.OperationKey);
            string webRootPath = root.WebRootPath;
            WebReport rep = new WebReport();

            rep.Report.Load(webRootPath + "/aiwa-data/" + DB + "/Template/Documents/" + details.Template);
            rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
            rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceHeader(DB, details.OperationKey), "Invoice Header");
            rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceDetails(DB, details.OperationKey), "Invoice Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceQR(DB, details.OperationKey), "Invoice QR");
            rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, transaction.AccountKey), "Current Account");
            rep.Report.RegisterData(CLiCore.Print.DDT.BankDetails(DB, details.Bank), "Bank Account");
            rep.Report.RegisterData(CLiCore.Print.DDT.SignaturDetails(DB, details.OperationKey), "Signatur Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");


            rep.EmbedPictures = true;
        
            ViewBag.WebReport = rep;
            return View();
        }
        public IActionResult PrintProformaInvoice(CLiCore.Print.PrintDetails details)
        {

            CLiCore.Print.PrintDetails.Insert(DB, details);
            CLiCore.Print.FinancialTransaction transaction = new CLiCore.Print.FinancialTransaction().GetProforma(DB, details.OperationKey);
            string webRootPath = root.WebRootPath;
            WebReport rep = new WebReport();

            rep.Report.Load(webRootPath + "/aiwa-data/" + DB + "/Template/Documents/" + details.Template);
            rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
            rep.Report.RegisterData(CLiCore.Print.DDT.Proforma(DB, details.OperationKey), "Invoice Header");
            rep.Report.RegisterData(CLiCore.Print.DDT.ProformaDetails(DB, details.OperationKey), "Invoice Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, transaction.AccountKey), "Current Account");
            rep.Report.RegisterData(CLiCore.Print.DDT.BankDetails(DB, details.Bank), "Bank Account");
            rep.Report.RegisterData(CLiCore.Print.DDT.SignaturDetails(DB, details.OperationKey), "Signatur Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");


            rep.EmbedPictures = true;
            rep.ShowPdfExport = false;
            ViewBag.WebReport = rep;
            return View();
        }
        public IActionResult PrintGeneralLedger(Guid? Key)
        {
            CLiCore.Print.FinancialTransaction transaction = new CLiCore.Print.FinancialTransaction().GetItem(DB, Key);

            string webRootPath = root.WebRootPath;
            WebReport rep = new WebReport();
            rep.Report.Load(webRootPath + "/aiwa-data/" + DB + "/Template/Documents/GeneralLedger.frx");
            rep.Report.RegisterData(DDT.CompanyProfile(DB), "Company Profile");
            rep.Report.RegisterData(DDT.GL(DB,Key), "GL Header");
            rep.Report.RegisterData(DDT.GLDetails(DB,Key), "GL Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.Transaction(DB, Key), "Transaction Header");
            rep.Report.RegisterData(CLiCore.Print.DDT.BranchDetails(DB, transaction.Branch), "Branch Details");
            rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");
            rep.EmbedPictures = true;
            rep.ShowPdfExport = false;
            ViewBag.WebReport = rep;
            return View();
        }



        //public IActionResult Pdf(Guid Key)
        //{
        //    //CLiCore.Print.PrintDetails details = new CLiCore.Print.PrintDetails().GetList(DB, Key);

        //    CLiCore.Print.FinancialTransaction transaction = new CLiCore.Print.FinancialTransaction().GetItem(DB, Key);
        //    string webRootPath = root.WebRootPath;
        //    WebReport rep = new WebReport();

        //    rep.Report.Load(webRootPath + "/aiwa-data/" + DB + "/Template/Documents/SalesInvoice.frx");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.CompanyProfile(DB), "Company Profile");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceHeader(DB, Key), "Invoice Header");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceDetails(DB, Key), "Invoice Details");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.InvoiceQR(DB, Key), "Invoice QR");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.CurrentDetails(DB, transaction.AccountKey), "Current Account");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.BankDetails(DB, null), "Bank Account");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.SignaturDetails(DB, Key), "Signatur Details");
        //    rep.Report.RegisterData(CLiCore.Print.DDT.UserDetails(transaction.CreateUser), "User Details");


        //    rep.Report.Prepare();
        //    FastReport.Export.Pdf.PDFExport export = new FastReport.Export.Pdf.PDFExport();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        export.EmbeddingFonts = true;
                
        //        export.Export(rep.Report, ms);
        //        ms.Flush();
        //        return File(ms.ToArray(), "application/pdf", Path.GetFileNameWithoutExtension("Simple List") + ".pdf");
        //    }
        //}
    }
}

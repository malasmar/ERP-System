using CLiAccounting.Reports;
using CLiCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;

namespace Platxe.Controllers
{
   // [Authorize]
    public class TemplatesController : Controller
    {
        //private readonly string DB;
        //private readonly int UserID;
        //private readonly string xLan;
        //public TemplatesController(IHttpContextAccessor httpContextAccessor)
        //{
        //    DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
        //    UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        //    xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        //}
        public IActionResult SalesInvoice(string DB,string xLan,Guid? Key)
        {
            InvoiceData invoice = new InvoiceData();
            CLiInventory.Documents.Transaction transaction = new CLiInventory.Documents.Transaction().GetItem(DB, Key);
            List<CLiInventory.Documents.TransactionDetails> details = new CLiInventory.Documents.TransactionDetails().GetList(DB,xLan, Key);
            CLiCore.Print.CurrentAccountDetails currentAccount = new CLiCore.Print.CurrentAccountDetails().GetItem(DB, transaction.CurrentKey);
            invoice.Transaction = transaction;
            invoice.Details = details;
            invoice.CurrentAccount = currentAccount;
            invoice.DB = DB;
            invoice.xLan = xLan;
            invoice.QR = iCore.QR(DB, transaction.InvoiceDate.Value, details.Sum(x => x.Total), details.Sum(x => x.vatAmount));
            return View(invoice);
        }
        public IActionResult Quotation(string DB, string xLan, Guid? Key)
        {
            QuotationData invoice = new QuotationData();
            CLiSales.Documents.Quotation transaction = new CLiSales.Documents.Quotation().GetItem(DB, Key);
            List<CLiSales.Documents.QuotationDetails> details = new CLiSales.Documents.QuotationDetails().GetList(DB, xLan, Key);
            CLiCore.Print.CurrentAccountDetails currentAccount = new CLiCore.Print.CurrentAccountDetails().GetItem(DB, transaction.Client);
            invoice.Transaction = transaction;
            invoice.Details = details;
            invoice.CurrentAccount = currentAccount;
            invoice.DB = DB;
            invoice.xLan = xLan;
 
            return View(invoice);
        }
        public IActionResult AccountStatment(string DB, string xLan, Guid? Key)
        {
            DateTime FirstDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime LastDate = new DateTime(DateTime.Now.Year, 12, 31);
            AccountStatmentData data = new AccountStatmentData();
            data.Statment = new CLiFinancial.Reports.AccountStatment.CurrentAccount().GetList(DB, Key, FirstDate, LastDate, false);
            data.Account = new CLiCore.Print.CurrentAccountDetails().GetItem(DB, Key);
            data.FirstDate= FirstDate;
            data.LastDate= LastDate;
            data.DB = DB;
            data.xLan = xLan;
            return View(data);
        }
        public IActionResult DownloadAccountStatment(string DB, string xLan, Guid Key)
        {

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 1450;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.MarginBottom = 5;
            converter.Options.MarginLeft = 5;
            converter.Options.MarginRight = 5;
            converter.Options.MarginTop = 5;
            converter.Options.KeepImagesTogether = true;
            converter.Options.KeepTextsTogether = true;
            converter.Options.EmbedFonts = true;
            converter.Options.DrawBackground = true;

            //PdfTextSection sec = new PdfTextSection(0, 50, "Page: {page_number} of {total_pages}.", new System.Drawing.Font("") );
            //sec.ForeColor = System.Drawing.Color.Blue;
            //converter.Footer.Add(sec);

            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl("http://localhost:5050/Templates/AccountStatment?DB=" + DB + "&xLan=" + xLan + "&Key=" + Key);



            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            doc.Close();
           
            //sw.WriteLine(str.ToString(), UnicodeEncoding.UTF8);
            return File(ms.GetBuffer(), "application/pdf", "Account Statment.pdf");
        }
    }
 
}

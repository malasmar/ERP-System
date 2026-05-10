using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using CLiCore;
using SelectPdf;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Net.Mail;
using System.Net;
using FastReport;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Platxe.Controllers.api
{
    [Produces("application/json")]
    [Route("api/[Controller]/[Action]")]
    [Authorize]
    public class icoreController : Controller
    {
        private readonly string DB;
        private readonly Guid? UserKey;
        private readonly string xLan;
        private readonly Guid? Subscribe;
        private readonly int Year;
        private readonly string UserEmail;
        public icoreController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            UserEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }

        [HttpGet]
        public JsonResult Databases()
        {
            return Json(new CLiCore.Account.Selections.Database().GetList(xLan, Subscribe));
        }
        [HttpGet]
        public JsonResult Years()
        {
            return Json(new CLiCore.Account.Selections.Years().GetList(DB));
        }

        [HttpGet]
        public JsonResult LoadAccountTypes(int Kind)
        {
            switch (Kind)
            {
                case (int)PLenums.TransactionAccount.CurrentAccount:
                    return Json(iReplacer.finCurrentAccountKind(xLan));
                case (int)PLenums.TransactionAccount.Employee:
                    return Json(iReplacer.finEmployeeAcccountTypes(xLan));
                default:
                    return Json(iReplacer.FinGeneralAccountTypes(xLan));
            }
        }

        [HttpGet]
        public JsonResult LoadAccounts(PLenums.TransactionAccount Kind, PLenums.CurrentAccountKind CurrentKind)
        {
            return Json(CLiFinancial.Selections.Dynamic.TransactionAccount(DB, xLan, Kind, CurrentKind));
        }


        private void SendEmail(string url, string FileName, Guid? Key)
        {
            if (Key == null)
                return;

            //CLiData.iSales.Quotation quotation = new CLiData.iSales.Quotation().GetItem(Key);
            CLiCore.Configuration.SenderEmail sender = new CLiCore.Configuration.SenderEmail().GetItem();
            if (sender.Email == "")
            {
                return;
            }
            if (CLiCore.iCore.IsValidEmail("shllash@hotmail.com") == false)
            {
                return;
            }


            string result;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            Thread T1 = new Thread(delegate ()
            {


                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = sender.UseDefaultCredentials;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = sender.Port;
                    smtp.EnableSsl = sender.EnableSsl;
                    var message = new MailMessage();
                    message.To.Add(new MailAddress("shllash@hotmail.com"));
                    message.From = new MailAddress(sender.Email);  // replace with valid value
                    message.Subject = "Quotation";
                    message.Body = result.ToString();
                    message.IsBodyHtml = sender.IsBodyHtml;
                    message.Attachments.Add(new Attachment(SavePDF(url), FileName + ".pdf", "application/pdf"));
                    var credential = new NetworkCredential
                    {
                        UserName = sender.UserName,  // replace with valid value
                        Password = sender.Password  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = sender.SMTP;
                    smtp.Send(message);
                }
            });
            T1.Start();
        }

        public Stream SavePDF(string url)
        {
            // read parameters from the webpage
            //  string url = "http://localhost:50801/Templates/Quotation01";

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
            //converter.Options.WebPageWidth = webPageWidth;
            //converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);
            MemoryStream ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            doc.Close();
            return ms;
        }

        [HttpGet]
        public JsonResult MaxMonthlyInvoice(int DocumentKind, int Year, int Month)
        {
            return Json(VoucherOperation.GetMaxMonthlyInvoices(DB, Year, Month, DocumentKind));
        }
        [HttpGet]
        public JsonResult MaxMonthlyVoucher(int DocumentKind, int Year, int Month)
        {
            return Json(VoucherOperation.GetMaxMonthlyVouchers(DB, Year, Month, DocumentKind));
        }

        [HttpPost]
        public JsonResult UpdateTransactionStatus(Guid Key, int Status)
        {
            CLiCore.VoucherOperation.UpdateTransactionStatus(DB, Key, Status);
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateQuotationStatus(Guid Key, int Status)
        {
            //if (Status == 1)
            //{
            //    new xmCore().SendDocumentEmail("http://localhost:5050/Templates/Quotation?DB=" + DB + "&xLan=" + xLan + "&Key=" + Key, "Quotation", Key);
            //}
            CLiCore.VoucherOperation.UpdateQuotationStatus(DB, Key, Status);
            return Json(true);
        }

        [HttpGet]
        public JsonResult PrintTemplates(int DocKind)
        {
            return Json(new CLiCore.Print.Templates().GetList(DB, DocKind));
        }
        [HttpGet]
        public JsonResult vatRate()
        {
            return Json(CLiFinancial.Selections.vatRate.vatRates(DB, xLan));
        }


        [HttpPost]
        public JsonResult DeleteFinancialTransaction(Guid Key)
        {
            bool res = CLiCore.VoucherOperation.DeleteFinancialTransaction(DB, Key);
            return Json(res);
        }
        [HttpPost]
        public JsonResult DeleteInventoryTransaction(Guid Key)
        {
            bool res = CLiCore.VoucherOperation.DeleteInventoryTransaction(DB, Key);
            return Json(res);
        }
        [HttpGet]
        public JsonResult ItemBalanceMainUnit(Guid? Key, int Warehouse, DateTime? Date)
        {

            return Json(CLiInventory.core.ItemBalanceMainUnit(DB, Key, Warehouse, Date));
        }

        [HttpPost]
        public JsonResult CheckPassword(string Password)
        {
            return Json(CLiCore.Account.core.CheckUserPassword(UserKey, Password));
        }

        [HttpPost]
        public JsonResult ChangePassword(string Password)
        {
            CLiCore.Account.core.UpdateUserPassword(UserKey, Password);
            return Json(true);
        }

        [HttpGet]
        public JsonResult GetServiceVATKey(Guid? Key)
        {
            return Json(CLiInventory.core.GetServiceVat(DB, Key));
        }
        [HttpGet]
        public JsonResult GetAccountBalance(Guid? Key, DateTime Date, int Kind, int Type)
        {
            if (Kind != 1)
                Type = -1;
            return Json(CLiFinancial.core.AccountBalance(DB, Key, Date, Type));
        }


       
    }
}

using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using CLiCore;

namespace Platxe.Areas.Financial.Controllers.api
{
    [Area("Financial")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/fin/[Controller]/[Action]")]
    public class idocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly static object Locker = new object();
        public idocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        [HttpPut]
        public JsonResult fackUpdate(int key, string values)
        {
            return Json(null);
        }

        [HttpGet]
        public JsonResult TransactionDetails(Guid? Key)
        {
            return Json(new CLiFinancial.Documents.TransactionDetails().GetList(DB, xLan, Key));
        }
        [HttpGet]
        public JsonResult TransactionDetailsJV(Guid? Key)
        {
            return Json(new CLiFinancial.Documents.TransactionDetails().GetListJV(DB, xLan, Key));
        }
        [HttpGet]
        public JsonResult Transactions(int Year, int DocumentKind, DataSourceLoadOptions loadOptions)
        {
            CLiFinancial.Documents.Manager.Transactions cls = new CLiFinancial.Documents.Manager.Transactions();
            return Json(DataSourceLoader.Load(cls.GetList(DB, Year, DocumentKind), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateTransaction(CLiFinancial.Documents.Transaction Header, string Data, bool isNew,string Invoices)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupUser = UserID;
                    Header.LastupDate = DateTime.Now;
                    OperationResult result;
                    List<CLiFinancial.Documents.TransactionDetails> details = new List<CLiFinancial.Documents.TransactionDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiFinancial.Documents.TransactionDetails>>(Data);

                    List<CLiFinancial.Documents.TransactionInvoices> invoices = new List<CLiFinancial.Documents.TransactionInvoices>();
                    invoices = JsonConvert.DeserializeObject<List<CLiFinancial.Documents.TransactionInvoices>>(Invoices);

                    result = CLiFinancial.Documents.core.UpdateTransaction(DB, Header, details, isNew, invoices);
                   
                    return Json(result);
                }
                catch (Exception ex)
                {
                    OperationResult result = new OperationResult();
                    result.Status = true;
                    result.Message = ex.Message;
                    return Json(result);
                }
            }
        }
        [HttpPost]
        public JsonResult UpdateJournalVoucher(CLiFinancial.Documents.Transaction Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupUser = UserID;
                    Header.LastupDate = DateTime.Now;
                    OperationResult result;
                    var details = new List<CLiFinancial.Documents.TransactionDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiFinancial.Documents.TransactionDetails>>(Data);

                    result = CLiFinancial.Documents.core.UpdateJournalVoucher(DB, Header, details, isNew);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    OperationResult result = new OperationResult();
                    result.Status = true;
                    result.Message = ex.Message;
                    return Json(result);
                }
            }
        }

        [HttpPost]
        public JsonResult PostSalaries(CLiFinancial.Documents.Transaction Header, int Year,int Month)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupUser = UserID;
                    Header.LastupDate = DateTime.Now;
                    OperationResult result;
                    

                    result = CLiFinancial.Documents.core.PostSalaries(DB, Header, Year, Month);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    OperationResult result = new OperationResult();
                    result.Status = true;
                    result.Message = ex.Message;
                    return Json(result);
                }
            }
        }

        #region "Selections"
        [HttpGet]
        public JsonResult AgesInvoices(int DocumentKind, Guid? Key, DataSourceLoadOptions loadOptions)
        {
            CLiFinancial.Documents.Selections.AgesCloseInvoices cls = new CLiFinancial.Documents.Selections.AgesCloseInvoices();
            return Json(DataSourceLoader.Load(cls.GetList(DB, DocumentKind, Key), loadOptions));
        }
        #endregion
        [HttpGet]
        public IActionResult TransactionCloseInvoices(Guid? Key)
        {
            return Json(new CLiFinancial.Documents.TransactionInvoices().GetList(DB, Key));
        }

        [HttpGet]
        public JsonResult SalariesPost(int Year, int Month ,DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Operation.SalariesPost().GetList(DB, Year,Month), loadOptions));
        }

        [HttpGet]
        public JsonResult SalariesPayment(int Year, int Month, string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Operation.SalariesPayment().GetList(DB, Year, Month, PaymentKind), loadOptions));
        }
    }
}

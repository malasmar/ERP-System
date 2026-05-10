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
using System.Security.Policy;

namespace Platxe.Areas.Sales.Controllers.api
{
    [Area("Sales")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Sales/[Controller]/[Action]")]
    public class idocumentsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid? UserKey;
        private readonly static object Locker = new object();
        public idocumentsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserKey =Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }

        [HttpPut]
        public JsonResult fackUpdate(int key, string values)
        {
            return Json(null);
        }

        [HttpGet]
        public JsonResult Invoices(int Year, int DocumentKind, DataSourceLoadOptions loadOptions)
        {
            CLiInventory.Documents.Manager.Invoices cls = new CLiInventory.Documents.Manager.Invoices();
            return Json(DataSourceLoader.Load(cls.GetList(DB, UserKey, Year, DocumentKind), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateSalesInvoice(CLiInventory.Documents.Transaction Header, string Data, bool isNew,string AdvancePayments)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupDate = DateTime.Now;
                    Header.LastupUser = UserID;
                    OperationResult result;
                    List<CLiInventory.Documents.TransactionDetails> details = new List<CLiInventory.Documents.TransactionDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiInventory.Documents.TransactionDetails>>(Data);

                    //bool CheckBalance = false;
                    //StringBuilder str = new StringBuilder();

                    //foreach (CLiInventory.Documents.TransactionDetails item in details)
                    //{
                    //    if (item.ItemKind == (int)PLenums.TransactionAccount.Stock)
                    //    {
                    //        decimal balance = 0;
                    //        balance = CLiInventory.core.ItemBalanceUnitFast(DB, item.ItemKey, Header.SourceWarehouse, item.Unit);
                    //        if ((item.Quantity + item.Bonus) > balance)
                    //        {
                    //            CheckBalance = true;
                    //            str.AppendLine("There is no balance for item in line : <b>" + item.Index + "</b> item code is <b> " + item.jnAccount.Code + "</b> <br />");
                    //        }
                    //    }
                    //}
                    //if (CheckBalance == true)
                    //{
                    //    result = new OperationResult();
                    //    result.Status = true;
                    //    result.Message = str.ToString();
                    //    return Json(result);
                    //}

                    List<CLiInventory.Documents.Selections.AdvancePayments> advancePayment = new List<CLiInventory.Documents.Selections.AdvancePayments>();
                    advancePayment = JsonConvert.DeserializeObject<List<CLiInventory.Documents.Selections.AdvancePayments>>(AdvancePayments);


                    result = CLiInventory.Documents.core.UpdateSalesInvoice(DB, Header, details, isNew, advancePayment);
                    // new xmCore().SendDocumentEmail("http://localhost:5050/Templates/SalesInvoice?DB=" + DB + "&xLan=" + xLan + "&Key=" + result.OperationKey, "Sales Invoice", result.OperationKey);  
                    // CLiCore.WaMessageSender.SendWa("+966501488719");
                    //xmCore xm = new xmCore();



                    //Thread T1 = new Thread(async delegate ()
                    //{
                    //    StringBuilder str = new StringBuilder();
                    //    str.AppendLine("Sales Invoice");
                    //    str.AppendLine("Total Invoice: " + details.Sum(x => x.Total).ToString("n2"));
                    //    str.Append(Header.Description);
                    //    await xm.SendWhatsapp(DB, "+966501488719", str.ToString());
                    //});
                    //T1.Start();


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
        public JsonResult UpdateReturnInvoice(CLiInventory.Documents.Transaction Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupDate = DateTime.Now;
                    Header.LastupUser = UserID;
                    OperationResult result;
                    var details = new List<CLiInventory.Documents.TransactionDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiInventory.Documents.TransactionDetails>>(Data);

                    result = CLiInventory.Documents.core.UpdateReturnSalesInvoice(DB, Header, details, isNew);
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
        [HttpGet]
        public JsonResult TransactionDetails(Guid? Key)
        {
            return Json(new CLiInventory.Documents.TransactionDetails().GetList(DB, xLan, Key));
        }



        //[HttpGet]
        //public JsonResult Quotations(int Year, int DocumentKind, DataSourceLoadOptions loadOptions)
        //{
        //    CLiSales.Documents.Manager.Invoices cls = new CLiInventory.Documents.Manager.Invoices();
        //    return Json(DataSourceLoader.Load(cls.GetList(DB, Year, DocumentKind), loadOptions));
        //}

        [HttpGet]
        public JsonResult Quotations(int Year, DataSourceLoadOptions loadOptions)
        {
            CLiSales.Documents.Quotation cls = new CLiSales.Documents.Quotation();
            return Json(DataSourceLoader.Load(cls.GetList(DB, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult Proforma(int Year, DataSourceLoadOptions loadOptions)
        {
            CLiSales.Documents.Proforma cls = new CLiSales.Documents.Proforma();
            return Json(DataSourceLoader.Load(cls.GetList(DB, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult Contracts(int Year, DataSourceLoadOptions loadOptions)
        {
            CLiSales.Documents.Contract cls = new CLiSales.Documents.Contract();
            return Json(DataSourceLoader.Load(cls.GetList(DB, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult QutationDetails(Guid? Key)
        {
            return Json(new CLiSales.Documents.QuotationDetails().GetList(DB, xLan, Key));
        }
        [HttpPost]
        public JsonResult UpdateQuotation(CLiSales.Documents.Quotation Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupDate = DateTime.Now;
                    Header.LastupUser = UserID;
                    OperationResult result;
                    var details = new List<CLiSales.Documents.QuotationDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiSales.Documents.QuotationDetails>>(Data);

                    result = CLiSales.Documents.core.UpdateQuotation(DB, Header, details, isNew);
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


        [HttpGet]
        public JsonResult ProformaInvoiceDetails(Guid? Key)
        {
            return Json(new CLiSales.Documents.ProformaDetails().GetList(DB, xLan, Key));
        }
        [HttpPost]
        public JsonResult UpdateProformaInvoice(CLiSales.Documents.Proforma Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupDate = DateTime.Now;
                    Header.LastupUser = UserID;
                    OperationResult result;
                    var details = new List<CLiSales.Documents.ProformaDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiSales.Documents.ProformaDetails>>(Data);

                    result = CLiSales.Documents.core.UpdateProformaInvoice(DB, Header, details, isNew);
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

        [HttpGet]
        public JsonResult ContractDetails(Guid? Key)
        {
            return Json(new CLiSales.Documents.ContractDetails().GetList(DB, xLan, Key));
        }
        [HttpPost]
        public JsonResult UpdateContract(CLiSales.Documents.Contract Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    Header.LastupDate = DateTime.Now;
                    Header.LastupUser = UserID;
                    OperationResult result;
                    var details = new List<CLiSales.Documents.ContractDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiSales.Documents.ContractDetails>>(Data);

                    result = CLiSales.Documents.core.UpdateContract(DB, Header, details, isNew);
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
        public JsonResult SelectInvoices(int DocumentKind, DataSourceLoadOptions loadOptions)
        {
            CLiInventory.Documents.Selections.Invoices cls = new CLiInventory.Documents.Selections.Invoices();
            return Json(DataSourceLoader.Load(cls.GetList(DB, DocumentKind), loadOptions));
        }
        [HttpGet]
        public JsonResult InvoiceDetails(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            CLiInventory.Documents.Selections.InvoiceDetails cls = new CLiInventory.Documents.Selections.InvoiceDetails();
            return Json(DataSourceLoader.Load(cls.GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult AdvanceInvoicePayments(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Documents.Selections.AdvancePayments().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult SelectQuotations(Guid? Key, DataSourceLoadOptions loadOptions)
        {
         
            return Json(DataSourceLoader.Load(new CLiSales.Selections.Quotations().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult SelectQuotationDetails(Guid? Key, DataSourceLoadOptions loadOptions)
        {

            return Json(DataSourceLoader.Load(new CLiSales.Selections.QuotationDetails().GetList(DB, Key), loadOptions));
        }

        [HttpGet]
        public JsonResult SelectContracts(Guid? Key, DataSourceLoadOptions loadOptions)
        {

            return Json(DataSourceLoader.Load(new CLiSales.Selections.Contracts().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult SelectContractDetails(Guid? Key, DataSourceLoadOptions loadOptions)
        {

            return Json(DataSourceLoader.Load(new CLiSales.Selections.ContractDetails().GetList(DB, Key), loadOptions));
        }


        [HttpGet]
        public JsonResult SelectProforma(Guid? Key, DataSourceLoadOptions loadOptions)
        {

            return Json(DataSourceLoader.Load(new CLiSales.Selections.Proforma().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult SelectProformaDetails(Guid? Key, DataSourceLoadOptions loadOptions)
        {

            return Json(DataSourceLoader.Load(new CLiSales.Selections.ProformaDetails().GetList(DB, Key), loadOptions));
        }
        #endregion





    }
}

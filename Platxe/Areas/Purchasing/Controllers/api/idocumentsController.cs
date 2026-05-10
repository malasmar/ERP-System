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
namespace Platxe.Areas.Purchasing.Controllers.api
{

    [Area("Purchasing")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Purchasing/[Controller]/[Action]")]
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
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }
        [HttpGet]
        public JsonResult Invoices(int Year, int DocumentKind, DataSourceLoadOptions loadOptions)
        {
            CLiInventory.Documents.Manager.Invoices cls = new CLiInventory.Documents.Manager.Invoices();
            return Json(DataSourceLoader.Load(cls.GetList(DB, UserKey, Year, DocumentKind), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdatePurchaseInvoice(CLiInventory.Documents.Transaction Header, string Data, bool isNew)
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

                    result = CLiInventory.Documents.core.UpdatePurchaseInvoice(DB, Header, details, isNew);
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

                    bool CheckBalance = false;
                    StringBuilder str = new StringBuilder();

                    foreach (CLiInventory.Documents.TransactionDetails item in details)
                    {
                        if (item.ItemKind == (int)PLenums.TransactionAccount.Stock)
                        {
                            decimal balance = 0;
                            balance = CLiInventory.core.ItemBalanceUnitFast(DB, item.ItemKey, Header.SourceWarehouse, item.Unit);
                            if ((item.Quantity + item.Bonus) > balance)
                            {
                                CheckBalance = true;
                                str.AppendLine("There is no balance for item in line : <b>" + item.Index + "</b> item code is <b> " + item.jnAccount.Code + "</b> <br />");
                            }
                        }
                    }
                    if (CheckBalance == true)
                    {
                        result = new OperationResult();
                        result.Status = true;
                        result.Message = str.ToString();
                        return Json(result);
                    }

                    result = CLiInventory.Documents.core.UpdateReturnPurchaseInvoice(DB, Header, details, isNew);
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
        public JsonResult SearchItems(string Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Selections.SearchItems().GetList(DB,xLan, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult SearchItemsCheck(string Key)
        {
            return Json(new CLiInventory.Selections.SearchItems().GetList(DB, xLan, Key));
        }
        #endregion
    }
}

using CLiCore.Account.Selections;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Platxe.Areas.Auditing.Controllers.api
{
    [Area("Auditing")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Auditing/[Controller]/[Action]")]
    public class icoreController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public icoreController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        [HttpGet]
        public JsonResult CloseFinancialYear(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiAccounting.Audit.CloseFinancialYear().GetList(DB, Year), loadOptions));
        }

        [HttpPost]
        public JsonResult CloseYear(int Year,string Description)
        {
            try
            {

                CLiCore.OperationResult result = new CLiCore.OperationResult();

                bool res = CLiAccounting.core.CloseFinancialYear(DB, Year, Description);
             
                result.Status = true;
                return Json(result);
            }
            catch (Exception ex)
            {
                CLiCore.OperationResult result = new CLiCore.OperationResult();
                result.Status = false;
                result.Message = ex.Message;
                return Json(result);
            }

           
        }

        [HttpGet]
        public JsonResult ItemsCost(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.Cost.ItemsCost().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult ItemCostDetails(Guid? Key,DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.Cost.ItemCostDetails().GetList(DB,Key), loadOptions));
        }
        [HttpGet]
        public JsonResult ItemsCostPrices(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.Cost.ItemsCostPrices().GetList(DB, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult TransactionsCost(int DocKind,int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.Cost.TransactionsCost().GetList(DB,DocKind, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult SalesInvoicesKeys(DateTime? Date)
        {
            return Json(new CLiInventory.Audit.Cost.InvoicesKeys().Sales(DB, Date));
        }
        [HttpGet]
        public JsonResult PurchaseInvoicesKeys(DateTime? Date)
        {
            return Json(new CLiInventory.Audit.Cost.InvoicesKeys().Purchasing(DB, Date));
        }
        [HttpPost]
        public JsonResult DeleteTransactionCost(DateTime Date)
        {
            int res = CLiInventory.core.DeleteItemsCost(DB, Date);
            if (res == 0)
                return Json(false);
            else
                return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateTransactionCost(Guid? Key,int DocKind,DateTime StartDate)
        {
            //try
            //{
            //CLiInventory.Documents.Transaction Header = null;
            //List<CLiInventory.Documents.TransactionDetails> Details = null;

            //Header = new CLiInventory.Documents.Transaction().GetItem(DB, Key);
            //Details = new CLiInventory.Documents.TransactionDetails().GetList(DB, Key);
            //CLiInventory.Documents.core.InsertReceiptInWarehouse(DB, Header, Details);
            CLiInventory.core.UpdateItemsCost(DB, Key, DocKind,StartDate);
            //}
            //catch(Exception e)
            //{
            //    System.IO.File.WriteAllText(@"D:\Hello.txt", Key.ToString() + " | " + DocKind.ToString());
            //}

            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateTransaction(int Year)
        {

            List<CLiInventory.Audit.Cost.InvoicesKeys> items = new CLiInventory.Audit.Cost.InvoicesKeys().Sales(DB, new DateTime(Year, 1, 1));
            foreach(CLiInventory.Audit.Cost.InvoicesKeys xitem in items)
            {
                CLiInventory.Documents.Transaction item = new CLiInventory.Documents.Transaction().GetItem(DB, xitem.Key);
                List<CLiInventory.Documents.TransactionDetails> details = new CLiInventory.Documents.TransactionDetails().GetList(DB, xitem.Key);
                switch (xitem.Kind)
                {
                    case (int)CLiCore.DocumentKind.PurchaseInvoice:
                        CLiInventory.Documents.clstempcore.UpdatePurchaseInvoice(DB, item, details, false);
                        break;
                    case (int)CLiCore.DocumentKind.ReturnPurchase:
                        CLiInventory.Documents.clstempcore.UpdateReturnPurchaseInvoice(DB, item, details, false);
                        break;

                    case (int)CLiCore.DocumentKind.SalesInvoice:
                        CLiInventory.Documents.clstempcore.UpdateSalesInvoice(DB, item, details);
                        break;
                    case (int)CLiCore.DocumentKind.ReturnSalesInvoice:
                        CLiInventory.Documents.clstempcore.UpdateReturnSalesInvoice(DB, item, details, false);
                        break;
                }
            }
        

            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateFinancialTransaction(int Year)
        {
            CLiFinancial.core.DeleteVATTransactions(DB, Year);
            List<CLiInventory.Audit.Cost.InvoicesKeys> invoices = new CLiInventory.Audit.Cost.InvoicesKeys().Invoices(DB, Year);
            foreach (CLiInventory.Audit.Cost.InvoicesKeys xitem in invoices)
            {
                CLiInventory.Documents.Transaction item = new CLiInventory.Documents.Transaction().GetItem(DB, xitem.Key);
                List<CLiInventory.Documents.TransactionDetails> details = new CLiInventory.Documents.TransactionDetails().GetList(DB, xitem.Key);
                switch (xitem.Kind)
                {
                    case (int)CLiCore.DocumentKind.PurchaseInvoice:
                        CLiInventory.Documents.clstempcore.UpdatePurchaseInvoice(DB, item, details, false);
                        break;
                    case (int)CLiCore.DocumentKind.ReturnPurchase:
                        CLiInventory.Documents.clstempcore.UpdateReturnPurchaseInvoice(DB, item, details, false);
                        break;

                    case (int)CLiCore.DocumentKind.SalesInvoice:
                        CLiInventory.Documents.clstempcore.UpdateSalesInvoice(DB, item, details);
                        break;
                    case (int)CLiCore.DocumentKind.ReturnSalesInvoice:
                        CLiInventory.Documents.clstempcore.UpdateReturnSalesInvoice(DB, item, details, false);
                        break;
                }
            }
            List<CLiInventory.Audit.Cost.InvoicesKeys> transactions = new CLiInventory.Audit.Cost.InvoicesKeys().Transactions(DB, Year);
            foreach (CLiInventory.Audit.Cost.InvoicesKeys xitem in transactions)
            {
                CLiFinancial.Documents.Transaction item = new CLiFinancial.Documents.Transaction().GetItem(DB, xitem.Key);
                List<CLiFinancial.Documents.TransactionDetails> details = new CLiFinancial.Documents.TransactionDetails().GetList(DB, "en", xitem.Key);
                switch (xitem.Kind)
                {
                    case (int)CLiCore.DocumentKind.finCashCollection:
                    case (int)CLiCore.DocumentKind.finCashPayment:
                    case (int)CLiCore.DocumentKind.finBankCollection:
                    case (int)CLiCore.DocumentKind.finBankPayment:
                    case (int)CLiCore.DocumentKind.finDebitNote:
                    case (int)CLiCore.DocumentKind.finCreditNote:
                        CLiFinancial.Documents.tempcore.UpdateTransaction(DB, item, details, false, new List<CLiFinancial.Documents.TransactionInvoices>());
                        break;
                    case (int)CLiCore.DocumentKind.finJournalVoucher:
                        CLiFinancial.Documents.tempcore.UpdateJournalVoucher(DB, item, details, false);
                        break;

                }
            }
            return Json(true);
        }

        [HttpGet]
        public JsonResult GeneralLedgerDiff(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiAccounting.Audit.GeneralLedgerDiff().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult ParentTransactions(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiAccounting.Audit.ParentTransactions().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult NullGeneralLedger(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiAccounting.Audit.NullGeneralLedger().GetList(DB), loadOptions));
        }

        [HttpGet]
        public JsonResult CountGLTransaction(Guid? Key)
        {
            return Json(CLiAccounting.core.CountGLTransaction(DB, Key));
        }
        public JsonResult UpdateGLTransfer(Guid? Source, Guid? Target)
        {
            CLiAccounting.core.TransferGLTransaction(DB, Source, Target);
            return Json(true);
        }

        [HttpGet]
        public JsonResult Stocktaking(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.Stocktaking().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult StocktakingDetails(Guid? Key,DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Audit.StocktakingDetails().GetList(DB, Key), loadOptions));
        }


        [HttpPost]
        public JsonResult OffsetStocktaking(Guid? Key,Guid? Account,string Comment)
        {
            try
            {
                CLiInventory.Audit.Stocktaking header = new CLiInventory.Audit.Stocktaking().GetItem(DB, Key);
                List<CLiInventory.Audit.StocktakingDetails> details = new CLiInventory.Audit.StocktakingDetails().GetList(DB, Key);

                return Json(CLiInventory.core.UpdateStocktakingOffset(DB, header, details, Account, Comment));
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return Json(false);
            }
         
        }
        [HttpPost]
        public JsonResult UpdateOpeningBalance(Guid? Key, string Comment)
        {
            CLiInventory.Audit.Stocktaking header = new CLiInventory.Audit.Stocktaking().GetItem(DB, Key);
            List<CLiInventory.Audit.StocktakingDetails> details = new CLiInventory.Audit.StocktakingDetails().GetList(DB, Key);

            return Json(CLiInventory.core.UpdateOpeningBalance(DB, header, details, Comment));
        }
        [HttpGet]
        public JsonResult UnpostedVouchers(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Operation.UnpostedVouchers().GetList(DB, Year), loadOptions));
        }
        [HttpPost]
        public JsonResult PostVoucher(Guid? Key)
        {
            CLiAccounting.core.PostVoucher(DB, Key);
            return Json(true);
        }

        [HttpGet]
        public JsonResult ItemsCostAudit(DateTime? FirstDate,DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Audit.ItemsCostAudit().GetList(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult ItemsCostAuditWarehouse(DateTime? FirstDate, DateTime? LastDate,int Warehouse, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Audit.ItemsCostAudit().GetList(DB, FirstDate, LastDate, Warehouse), loadOptions));
        }
        [HttpGet]
        public JsonResult WarehousesValues(DateTime? Date, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Audit.WarehousesValues().GetList(DB, Date), loadOptions));
        }

        //Check Integration
        [HttpGet]
        public JsonResult IntegrationCards(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.CheckIntegration.FinancialAccounts().GetList(DB), loadOptions));
        }
    }
}

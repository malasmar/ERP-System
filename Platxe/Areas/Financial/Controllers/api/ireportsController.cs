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
    public class ireportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid? UserKey;
        private readonly static object Locker = new object();
        public ireportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
        }
        [HttpGet]
        public JsonResult AccountStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountStatment.CurrentAccount().GetList(DB, Key, FirstDate, LastDate, Opening), loadOptions));
        }
        [HttpGet]
        public JsonResult EmployeeStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, string Kind, bool Opening, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountStatment.CurrentAccount().GetList(DB, Key, FirstDate, LastDate, Opening, Kind), loadOptions));
        }
        [HttpGet]
        public JsonResult FixtureStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, string Kind, bool Opening, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountStatment.CurrentAccount().GetList(DB, Key, FirstDate, LastDate, Opening, Kind), loadOptions));
        }
        [HttpGet]
        public JsonResult SummaryStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, bool Opening, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountStatment.StatmentSummary().GetList(DB, Key, FirstDate, LastDate, Opening), loadOptions));
        }
        //Balance
        [HttpGet]
        public JsonResult BalanceCurrentAccount(DateTime? FirstDate, DateTime? LastDate,string Parents, string Groups, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().CurrentBalance(DB, FirstDate, LastDate, Parents, Groups,UserKey), loadOptions));
        }
        [HttpGet]
        public JsonResult BalanceCash(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().CashBalance(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult BalanceBank(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().BankBalance(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult BalanceExpense(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().ExpenseBalance(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult BalanceRevenue(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().RevenueBalance(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult BalanceFixture(DateTime? FirstDate, DateTime? LastDate, string Kind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().FixtureBalance(DB, FirstDate, LastDate, Kind), loadOptions));
        }
        public JsonResult BalanceEmployee(DateTime? FirstDate, DateTime? LastDate, int Kind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.AccountBalance.CurrentAccount().EmployeeBalance(DB, FirstDate, LastDate, Kind), loadOptions));
        }
        [HttpGet]
        public JsonResult vatDeclaration(DateTime FirstDate, DateTime LastDate)
        {
            return Json(new CLiFinancial.Reports.vat.vatDeclaration().GetList(DB, xLan, FirstDate, LastDate));
        }
        [HttpGet]

        [HttpGet]
        public JsonResult vatStatmentDetails(DateTime FirstDate, DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.vat.vatStatment().GetListDetails(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult vatStatmentSummary(DateTime FirstDate, DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.vat.vatStatment().GetListSummary(DB, FirstDate, LastDate), loadOptions));
        }

        [HttpGet]
        public JsonResult DeclerationSheet(DateTime FirstDate, DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.vat.DeclerationSheet().GetList(DB, FirstDate, LastDate), loadOptions));
        }

        [HttpGet]
        public JsonResult AgesSummary(int DocKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Ages.AgesSummary().GetList(DB, DocKind), loadOptions));
        }
        [HttpGet]
        public JsonResult AgesDetails(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Ages.AgesDetails().GetList(DB, Key), loadOptions));
        }

        [HttpGet]
        public JsonResult CostCenterBalance(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.CostCenter.CostCenterBalance().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult CostCenterIncomeSummary(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.CostCenter.IncomeSummary().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult CostCenterStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.CostCenter.CostCenterStatment().GetList(DB, Key, FirstDate, LastDate), loadOptions));
        }

        [HttpGet]
        public JsonResult ProjectBalance(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Project.ProjectBalance().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult ProjectIncomeSummary(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Project.IncomeSummary().GetList(DB, Key), loadOptions));
        }
        [HttpGet]
        public JsonResult ProjectStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Project.ProjectStatment().GetList(DB, Key, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult TransactionDetails(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Reports.Advance.TransactionDetails().GetList(DB, Year), loadOptions));
        }
    }
}

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
namespace Platxe.Areas.Accounting.Controllers.api
{
    [Authorize]
    [Area("Accounting")]
    //[Produces("application/json")]
    [Route("api/acc/[Controller]/[Action]")]
    [ApiController]
    public class ireportsController : ControllerBase
    {
        private readonly string DB;
        private readonly string xLan;
        private readonly static object Locker = new object();
        public ireportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        
        [HttpGet]
        public object TrialBalance(DateTime First,DateTime Last,int Level,bool ExpKind,bool HideZero, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.TrialBalance().GetList(DB, First, Last, Level, ExpKind, HideZero), loadOptions);
        }
        [HttpGet]
        public object TrialBalanceTree(DateTime First, DateTime Last,bool ExpKind, bool HideZero, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.TrialBalance().GetTreeList(DB, First, Last, ExpKind, HideZero), loadOptions);
        }
        [HttpGet]
        public object IncomeStatment(DateTime First, DateTime Last, int GroupLevel, int TransactionLevel, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.IncomeStatment().GetList(DB, First, Last,GroupLevel, TransactionLevel), loadOptions);
        }
        [HttpGet]
        public object IncomeStatmentExpKind(DateTime First, DateTime Last, bool ExpKind, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.IncomeStatment().GetList(DB, First, Last, ExpKind), loadOptions);
        }
        [HttpGet]
        public object BalanceStatment(DateTime First, DateTime Last, int GroupLevel, int TransactionLevel, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.BalanceStatment().GetList(DB, First, Last, GroupLevel, TransactionLevel), loadOptions);
        }
        [HttpGet]
        public object TrialBalance_TL(DateTime First, DateTime Last)
        {
            return new CLiAccounting.Reports.TrialBalance().TransactionLevel(DB, First, Last);
        }

        [HttpGet]
        public object BalanceTB(DateTime First, DateTime Last)
        {
            return new CLiAccounting.Reports.BalanceTB().GetItem(DB, First, Last);
        }
        [HttpGet]
        public object AccountStatment(Guid? Key,DateTime FirstDate, DateTime LastDate, bool Opening, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.AccountStatment().GetList(DB, Key, FirstDate, LastDate, Opening), loadOptions);
        }
        [HttpGet]
        public object ParentStatment(string Key, DateTime FirstDate, DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.ParentStatment().GetList(DB, Key, FirstDate, LastDate), loadOptions);
        }
        [HttpGet]
        public object YearlyStatment(int Year, DataSourceLoadOptions loadOptions)
        {
            DateTime FirstDate=new DateTime(Year,1,1);
            DateTime LastDate=new DateTime(Year,12,31);   
            return DataSourceLoader.Load(new CLiAccounting.Reports.ParentStatment().YearlyStatment(DB,FirstDate, LastDate), loadOptions);
        }
        [HttpGet]
        public object ParentBalance(string Account,DateTime First, DateTime Last, int Level, bool ExpKind, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(new CLiAccounting.Reports.TreeBalance.ParentBalance().GetList(DB, Account, First, Last, ExpKind), loadOptions);
        }
    }
}

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

namespace Platxe.Areas.Financial.Controllers.api
{

    [Area("Financial")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/fin/[Controller]/[Action]")]
    public class icardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public icardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        [HttpGet]
        public JsonResult CurrentAccount(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.CurrentAccount().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateCurrentAccount(CLiFinancial.Cards.CurrentAccount data)
        {
            Guid? Key;
            if (data.Key == null)
            {
                Key = CLiFinancial.Cards.CurrentAccount.Insert(DB, data);
            }
            else
            {
                Key = CLiFinancial.Cards.CurrentAccount.Update(DB, data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteCurrentAccount(Guid? Key)
        {
            return Json(CLiFinancial.Cards.CurrentAccount.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Bank(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Bank().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateBank(CLiFinancial.Cards.Bank data)
        {
            Guid? Key;
            if (data.Key == null)
            {
                Key= CLiFinancial.Cards.Bank.Insert(DB, data);
            }
            else
            {
                Key= CLiFinancial.Cards.Bank.Update(DB, data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteBank(Guid? Key)
        {
            return Json(CLiFinancial.Cards.Bank.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult CashBox(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.CashBox().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateCashBox(CLiFinancial.Cards.CashBox data)
        {
            Guid? Key;
            if (data.Key == null)
            {
                Key=CLiFinancial.Cards.CashBox.Insert(DB, data);
            }
            else
            {
                Key=CLiFinancial.Cards.CashBox.Update(DB, data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteCashBox(Guid? Key)
        {
            return Json(CLiFinancial.Cards.CashBox.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Expenses(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Expenses().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateExpenses(CLiFinancial.Cards.Expenses data)
        {
            Guid? Key;
            if (data.Key == null)
            {
                Key=CLiFinancial.Cards.Expenses.Insert(DB, data);
            }
            else
            {
                Key= CLiFinancial.Cards.Expenses.Update(DB, data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteExpenses(Guid? Key)
        {
            return Json(CLiFinancial.Cards.Expenses.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Revenue(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Revenue().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateRevenue(CLiFinancial.Cards.Revenue data)
        {
            Guid? Key;
            if (data.Key == null)
            {
                Key=CLiFinancial.Cards.Revenue.Insert(DB, data);
            }
            else
            {
                Key= CLiFinancial.Cards.Revenue.Update(DB, data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteRevenue(Guid? Key)
        {
            return Json(CLiFinancial.Cards.Revenue.Delete(DB, Key));
        }
    }
}

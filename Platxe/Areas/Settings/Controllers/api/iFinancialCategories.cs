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
namespace Platxe.Areas.Settings.Controllers.api
{
  
    [Area("Settings")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Settings/[Controller]/[Action]")]
    public class iFinancialCategories : Controller
    {

        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public iFinancialCategories(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        [HttpGet]
        public JsonResult GrpCurrentAccount(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.CurrentAccountGroup().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateGrpCurrentAccount(CLiFinancial.Cards.Groups.CurrentAccountGroup data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.CurrentAccountGroup.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.CurrentAccountGroup.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteGrpCurrentAccount(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.CurrentAccountGroup.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GrpCashBox(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.CashboxGroup().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateGrpCashBox(CLiFinancial.Cards.Groups.CashboxGroup data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.CashboxGroup.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.CashboxGroup.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteGrpCashBox(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.CashboxGroup.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GrpExpenses(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.ExpensesGroup().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateGrpExpenses(CLiFinancial.Cards.Groups.ExpensesGroup data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.ExpensesGroup.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.ExpensesGroup.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteGrpExpenses(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.ExpensesGroup.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GrpRevenue(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.RevenueGroup().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateGrpRevenue(CLiFinancial.Cards.Groups.RevenueGroup data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.RevenueGroup.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.RevenueGroup.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteGrpRevenue(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.RevenueGroup.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult TransactionCategories(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.TransactionCategories().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateTransactionCategories(CLiFinancial.Cards.Groups.TransactionCategories data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.TransactionCategories.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.TransactionCategories.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteTransactionCategories(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.TransactionCategories.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult JVCategories(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.JVCategories().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateJVCategories(CLiFinancial.Cards.Groups.JVCategories data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.JVCategories.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.JVCategories.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteJVCategories(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.JVCategories.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult GrpActivity(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.Groups.Activity().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateGrpActivity(CLiFinancial.Cards.Groups.Activity data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.Groups.Activity.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.Groups.Activity.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteGrpActivity(Guid? Key)
        {
            int res = CLiFinancial.Cards.Groups.Activity.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult SalariesIntegration(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.Cards.SalariesIntegration().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateSalariesIntegration(CLiFinancial.Cards.SalariesIntegration data)
        {
            if (data.Key == null)
            {
                CLiFinancial.Cards.SalariesIntegration.Insert(DB, data);
            }
            else
            {
                CLiFinancial.Cards.SalariesIntegration.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteSalariesIntegration(Guid? Key)
        {
            int res = CLiFinancial.Cards.SalariesIntegration.Delete(DB, Key);
            return Json(res);
        }
    }
}

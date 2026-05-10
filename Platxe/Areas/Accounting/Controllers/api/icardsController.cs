using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
namespace Platxe.Areas.Accounting.Controllers.api
{
    

    [Area("Accounting")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/acc/[Controller]/[Action]")]
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
        public JsonResult AccountsRoot(DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Cards.Root cls = new CLiAccounting.Cards.Root();
            return Json(DataSourceLoader.Load(cls.GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateRoot(CLiAccounting.Cards.Root data)
        {
            if (data.Key == null)
            {
                CLiAccounting.Cards.Root.Insert(DB, data);
            }
            else
            {
                CLiAccounting.Cards.Root.Update(DB, data);
            }
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateAccount(CLiAccounting.Cards.Account data)
        {
            if (data.Key == null)
            {
                CLiAccounting.Cards.Account.Insert(DB, data);
            }
            else
            {
                CLiAccounting.Cards.Account.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteRoot(Guid? Key)
        {
             int res=  CLiAccounting.Cards.Root.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult ChartofAccount(DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Cards.ChartofAccounts cls = new CLiAccounting.Cards.ChartofAccounts();
            return Json(DataSourceLoader.Load(cls.GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult AccountDetails(string Key)
        {
            return Json(new CLiAccounting.Cards.Account().GetItem(DB, Key));
        }
        [HttpGet]
        public JsonResult AccountCode(string Parent)
        {
            return Json(CLiAccounting.core.AccountCode(DB, Parent));
        }
        [HttpGet]
        public JsonResult BalanceItemCode(string Parent)
        {
            return Json(CLiAccounting.core.BalanceItemCode(DB, Parent));
        }
        [HttpGet]
        public JsonResult BalanceSheetItems(DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Cards.BalanceSheetItems cls = new CLiAccounting.Cards.BalanceSheetItems();
            return Json(DataSourceLoader.Load(cls.GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateBalanceSheetItems(CLiAccounting.Cards.BalanceSheetItems data)
        {
            if (data.Key == null)
            {
                CLiAccounting.Cards.BalanceSheetItems.Insert(DB, data);
            }
            else
            {
                CLiAccounting.Cards.BalanceSheetItems.Update(DB, data);
            }
            return Json(true);
        }
        [HttpGet]
        public JsonResult TreeBalanceItem()
        {
            return Json(new CLiAccounting.Cards.Tree.BalanceItems().GetList(DB,"",xLan));
        }
        //Financial Cards
        [HttpGet]
        public JsonResult CostCenter(DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Cards.CostCenter cls = new CLiAccounting.Cards.CostCenter();
            return Json(DataSourceLoader.Load(cls.GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateCostCenter(CLiAccounting.Cards.CostCenter data)
        {
            if (data.Key == null)
            {
                CLiAccounting.Cards.CostCenter.Insert(DB, data);
            }
            else
            {
                CLiAccounting.Cards.CostCenter.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteCostCenter(Guid? Key)
        {
            int res = CLiAccounting.Cards.CostCenter.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Project(DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Cards.Project cls = new CLiAccounting.Cards.Project();
            return Json(DataSourceLoader.Load(cls.GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateProject(CLiAccounting.Cards.Project data)
        {
            if (data.Key == null)
            {
                CLiAccounting.Cards.Project.Insert(DB, data);
            }
            else
            {
                CLiAccounting.Cards.Project.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteProject(Guid? Key)
        {
            int res = CLiAccounting.Cards.Project.Delete(DB, Key);
            return Json(res);
        }
        [HttpGet]
        public JsonResult ChartOfAccountTree()
        {
            return Json(new CLiAccounting.Cards.Tree.ChartofAccounts().GetList(DB, "", xLan));
        }

        [HttpPut]
        public IActionResult UpdateCard(Guid? key, string values)
        {
            var Card =new CLiAccounting.Cards.ChartofAccounts().GetItem(DB, key);
            JsonConvert.PopulateObject(values, Card);
            CLiAccounting.Cards.Account.Update(DB, Card);
            return Ok();
        }

        [HttpPost]
        public JsonResult CreateChartofAccount(int ParentKey, string values)
        {
            try
            {

                var cls = new CLiAccounting.Cards.ChartofAccounts();
                JsonConvert.PopulateObject(values, cls);

                CLiAccounting.Cards.Account Parent = new CLiAccounting.Cards.Account().GetItem(DB, cls.ParentKey);

                CLiAccounting.Cards.Account NewAccount = new CLiAccounting.Cards.Account(); ;
                NewAccount.Code = cls.Code;
                NewAccount.Name1 = cls.Name1;
                NewAccount.Name2 = cls.Name2;
                NewAccount.Level = cls.Level;
                NewAccount.IsParent = false;
                NewAccount.BalanceItem = null;
                NewAccount.Kind = cls.Kind;
                NewAccount.Financial = false;
                NewAccount.Source = true;
                NewAccount.Category = cls.Category;
                NewAccount.Disable = false;
                NewAccount.Status = 0;
                NewAccount.Parent = Parent.Code;
                NewAccount.DC = false;
                NewAccount.CloseSubAccounts = false;
                CLiAccounting.Cards.Account.Insert(DB, NewAccount);
                return Json(Ok());
            }
            catch (Exception EX)
            {
                return Json("Error");
            }
        }

    }
}

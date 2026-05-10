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
namespace Platxe.Areas.FixedAssets.Controllers.api
{
   
    [Area("FixedAssets")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/fxd/[Controller]/[Action]")]
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
        public JsonResult Categories(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.FixedAssets.Cards.Categories().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateCategories(CLiFinancial.FixedAssets.Cards.Categories data)
        {
            if (data.Key == null)
            {
                CLiFinancial.FixedAssets.Cards.Categories.Insert(DB, data);
            }
            else
            {
                CLiFinancial.FixedAssets.Cards.Categories.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteCategories(Guid? Key)
        {
            return Json(CLiFinancial.FixedAssets.Cards.Categories.Delete(DB, Key));
        }

        [HttpGet]
        public JsonResult Fixture(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.FixedAssets.Cards.Fixture().GetList(DB), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateFixture(CLiFinancial.FixedAssets.Cards.Fixture data)
        {
            if (data.Key == null)
            {
                CLiFinancial.FixedAssets.Cards.Fixture.Insert(DB, data);
            }
            else
            {
                CLiFinancial.FixedAssets.Cards.Fixture.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteFixture(Guid? Key)
        {
            return Json(CLiFinancial.FixedAssets.Cards.Categories.Delete(DB, Key));
        }
        [HttpGet]
        public JsonResult FixtureCode(Guid? Key,string Prefix)
        {
            return Json(CLiCore.VoucherOperation.AssetsAccountCode(DB, Key, Prefix));
        }
    }
}

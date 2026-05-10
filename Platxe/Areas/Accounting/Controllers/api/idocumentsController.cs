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
    [Area("Accounting")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/acc/[Controller]/[Action]")]
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
        [HttpGet]
        public JsonResult LoadGeneralLedger(Guid? Key)
        {
            return Json(new CLiAccounting.Documents.GeneralLedgerDetails().GetList(DB, xLan, Key));
        }
        [HttpGet]
        public JsonResult DocumentsList(int Year,DataSourceLoadOptions loadOptions)
        {
            CLiAccounting.Documents.Transaction cls = new CLiAccounting.Documents.Transaction();
            return Json(DataSourceLoader.Load(cls.GetList(DB,Year), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateGeneralLedger(CLiAccounting.Documents.GeneralLedger Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    OperationResult result;
                    var details = new List<CLiAccounting.Documents.GeneralLedgerDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiAccounting.Documents.GeneralLedgerDetails>>(Data);
                
                    result = CLiAccounting.core.UpdateGeneralLedger(DB, Header, details, isNew);
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
    }
}

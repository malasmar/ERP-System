using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Platxe.Areas.Inventory.Controllers.api
{
    [Area("Inventory")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/inv/[Controller]/[Action]")]
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
        public JsonResult ItemUnits(Guid? Key)
        {
            return Json(new CLiInventory.Selections.Stock.ItemUnits().GetList(DB, Key));
        }
        [HttpGet]
        public JsonResult InvoiceItemDetails(Guid? Key)
        {
            return Json(new CLiInventory.JsonData.InvoiceItemDetails().GetItem(DB, Key));
        }

      

    }
}

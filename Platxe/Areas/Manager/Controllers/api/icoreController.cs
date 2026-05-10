using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Platxe.Areas.Manager.Controllers.api
{

    [Area("Manager")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Manager/[Controller]/[Action]")]
    public class icoreController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid Subscribe;
        private readonly IWebHostEnvironment root;
        private readonly string CID;
        public icoreController(IWebHostEnvironment hostingEnvironment,IHttpContextAccessor httpContextAccessor)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            CID = httpContextAccessor.HttpContext.User.FindFirstValue("aiwCID");
        }

        [HttpGet]
        public JsonResult Users(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Platx.Users().GetList(Subscribe), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateUser(CLiCore.Platx.Users data)
        {
            if (CLiCore.Platx.Users.CheckUserEmail(data.Email) > 0)
                return Json("ErrorEmial");

            if (CLiCore.iCore.IsValidEmail(data.Email) ==false)
                return Json("ErrorValidEmial");

            if (CLiCore.Platx.Users.CheckUserUsername(data.Phone) > 0)
                return Json("ErrorPhone");

            Guid? Key;
            if (data.Key == null)
            {
                Key = Guid.NewGuid();

               
                data.Key = Key;
                data.Subscribe = Subscribe;
                CLiCore.Platx.Users.Insert(data);
                string pass = CLiCore.iRandom.RandomDigits(6);
                CLiCore.Platx.Users.UpdatePassword(Key, pass);
                string message = System.IO.File.ReadAllText(Path.Combine(root.WebRootPath, "PL", "Whatsapp-CreateAccount-ar.txt"));
                message = message.Replace("##User", data.Name1);
                message = message.Replace("##Phone", data.Phone);
                message = message.Replace("##ComID", CID);
                message = message.Replace("##Email", data.Email);
                message = message.Replace("##Password", pass);
                new xmCore().SendWhatsappMesege(data.WhatsappNumber, message);

            }
            else
            {
                Key = data.Key;
                CLiCore.Platx.Users.Update(data);
            }
            return Json(Key);
        }
        [HttpDelete]
        public JsonResult DeleteUser(Guid? Key)
        {
            return Json(CLiCore.Platx.Users.Delete(Key));
        }



        [HttpGet]
        public JsonResult Technical(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Platx.Technical().GetList(Subscribe), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdateTechnical(CLiCore.Platx.Technical data)
        {
            if (data.Key == null)
            {
                CLiCore.Platx.Technical.Insert(DB, data);
            }
            else
            {
                CLiCore.Platx.Technical.Update(DB, data);
            }
            return Json(true);
        }
        [HttpDelete]
        public JsonResult DeleteTechnical(Guid? Key)
        {
            return Json(CLiCore.Platx.Technical.Delete(DB, Key));
        }

        //user Payment Methods
        [HttpGet]
        public JsonResult PaymentMethods(Guid? Key, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Platx.PaymentMethods().GetList(DB, Key), loadOptions));
        }
        [HttpPost]
        public JsonResult UpdatePaymentMethod(CLiCore.Platx.PaymentMethods data)
        {
            if (data.Key == null)
            {
                CLiCore.Platx.PaymentMethods.Insert(DB, data);
            }
            else
            {
                CLiCore.Platx.PaymentMethods.Update(DB, data);
            }
            return Json(true);
        }

        [HttpGet]
        public JsonResult LoadUserModules(Guid? Key)
        {
            return Json(new CLiCore.Platx.Modules().GetList(DB, xLan, 1, "", Key));
        }
        [HttpPost]
        public JsonResult UpdateUserModules(Guid? Key, string data)
        {
            List<CLiCore.Platx.Modules> dt = new List<CLiCore.Platx.Modules>();
            JsonConvert.PopulateObject(data, dt);
            CLiCore.Platx.Modules.InsertUserRights(DB, Key, dt);
            return Json(true);
        }

        [HttpPost]
        public JsonResult UpdateWhatsapp(CLiCore.Platx.Whatsapp data)
        {
            CLiCore.Platx.Whatsapp.Insert(DB, data);
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateEmail(CLiCore.Configuration.MyEmail data)
        {
            CLiCore.Configuration.MyEmail.Insert(DB, data);
            return Json(true);
        }
    }
}

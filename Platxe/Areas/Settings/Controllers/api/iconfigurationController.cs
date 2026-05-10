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
    public class iconfigurationController : Controller
    {
        private readonly IWebHostEnvironment root;
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public iconfigurationController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            root = hostingEnvironment;
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        [HttpPost]
        public JsonResult UpdateSettings(CLiCore.Configuration.Settings data)
        {
            CLiCore.Configuration.Settings.Insert(DB,data);
            return Json(true);
        }

        [HttpPost]
        public JsonResult UpdateCompanyProfile(IFormFile files, CLiCore.Configuration.Profile data)
        {
            try
            {
                CLiCore.Configuration.Profile.Insert(DB,data);
                if (files != null)
                {
                    using (var streamReader = new MemoryStream())
                    {
                        files.CopyTo(streamReader);
                        if (System.IO.File.Exists(Path.Combine(root.WebRootPath, "aiwa-data", DB, @"Content\Logo\" + files.FileName)))
                        {
                            System.IO.File.Delete(Path.Combine(root.WebRootPath, "aiwa-data", DB, @"Content\Logo\" + files.FileName));
                        }
                        System.IO.File.WriteAllBytes(Path.Combine(root.WebRootPath, "aiwa-data", DB, @"Content\Logo\" + files.FileName), streamReader.ToArray());
                        CLiCore.Configuration.Profile.UpdateLogo(DB, files.FileName, streamReader.ToArray());
                    }

                }
          
            }
            catch
            {
                Response.StatusCode = 400;
            }
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateDefaultSettings(CLiCore.Configuration.DefaultSettings data)
        {
            CLiCore.Configuration.DefaultSettings.Insert(DB, data);
            return Json(true);
        }
        [HttpGet]
        public JsonResult vatKind(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.vatKind().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdatevatKind(CLiCore.Configuration.vatKind data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.vatKind.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.vatKind.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeletevatKind(Guid? Key)
        {
            int res = CLiCore.Configuration.vatKind.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult vatRate(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.vatRates().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdatevatRate(CLiCore.Configuration.vatRates data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.vatRates.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.vatRates.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeletevatRate(Guid? Key)
        {
            int res = CLiCore.Configuration.vatRates.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult Branch(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.Branch().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateBranch(CLiCore.Configuration.Branch data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.Branch.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.Branch.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteBranch(Guid? Key)
        {
            int res = CLiCore.Configuration.Branch.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Prefix(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.Prefix().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdatePrefix(CLiCore.Configuration.Prefix data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.Prefix.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.Prefix.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeletePrefix(Guid? Key)
        {
            int res = CLiCore.Configuration.Prefix.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult Currency(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.Currency().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateCurrency(CLiCore.Configuration.Currency data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.Currency.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.Currency.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteCurrency(Guid? Key)
        {
            int res = CLiCore.Configuration.Currency.Delete(DB, Key);
            return Json(res);
        }

        [HttpGet]
        public JsonResult FinancialYear(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.FinancialYear().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateFinancialYear(CLiCore.Configuration.FinancialYear data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.FinancialYear.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.FinancialYear.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteFinancialYear(Guid? Key)
        {
            int res = CLiCore.Configuration.FinancialYear.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult City(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.City().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateCity(CLiCore.Configuration.City data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.City.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.City.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteCity(Guid? Key)
        {
            int res = CLiCore.Configuration.City.Delete(DB, Key);
            return Json(res);
        }


        [HttpGet]
        public JsonResult ClientCategories(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiCore.Configuration.ClientCategories().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateClientCategories(CLiCore.Configuration.ClientCategories data)
        {
            if (data.Key == null)
            {
                CLiCore.Configuration.ClientCategories.Insert(DB, data);
            }
            else
            {
                CLiCore.Configuration.ClientCategories.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteClientCategories(Guid? Key)
        {
            int res = CLiCore.Configuration.ClientCategories.Delete(DB, Key);
            return Json(res);
        }
        [HttpPost]
        public JsonResult UpdateEmployeeAccounts(CLiCore.Configuration.EmployeeAccounts data)
        {
          
                CLiCore.Configuration.EmployeeAccounts.Insert(DB, data);
         
            return Json(true);
        }
    }
}

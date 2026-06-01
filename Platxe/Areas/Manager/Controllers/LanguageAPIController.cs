using CLiCore;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Platxe.Areas.Manager.Controllers
{
    public class LanguageAPIController : Controller
    {
        private readonly string db;
        private readonly int UserID;
        public LanguageAPIController(IHttpContextAccessor httpContextAccessor)
        {
            db = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }


        [HttpGet]
        public object GetLanguage(int ItemKey, DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(clsLanguage.GetList(db), loadOptions);
        }


        [HttpPost]
        public JsonResult CreateLanguage(int ItemKey, string values)
        {
            try
            {

                var cls = new clsLanguage();
                JsonConvert.PopulateObject(values, cls);


                clsLanguage.Insert("", cls);
                return Json(Ok());
            }
            catch (Exception EX)
            {
                return Json("Error");
            }
        }

        [HttpPut]
        public JsonResult UpdateLanguage(int key, string values)
        {
            try
            {
                var cls = clsLanguage.GetItem("", key);
                JsonConvert.PopulateObject(values, cls);

                clsLanguage.Update("", cls);
                return Json(Ok());
            }
            catch (Exception EX)
            {
                return Json("Error");
            }
        }

        public IActionResult DeleteLanguage(int key)
        {
            try
            {
                bool result = clsLanguage.Delete("", key);

                if (!result)
                    return BadRequest("Already used cannot be deleted!");

                return Ok();
            }
            catch (Exception EX)
            {
                return Json("Error");
            }

        }
    }
}

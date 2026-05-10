using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace Platxe.Areas.HR.Controllers.api
{
    [Area("HR")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/hr/[Controller]/[Action]")]
    public class istructureController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid? Subscribe;
        public istructureController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            Subscribe =Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
        }
        [HttpGet]
        public JsonResult StructureList()
        {
            return Json(new CLiHR.Cards.Structure().GetList(DB));
        }
        [HttpGet]
        public JsonResult StructurexList()
        {
            return Json(new CLiHR.Selections.Structurex().GetList(DB, xLan));
        }
        [HttpGet]
        public JsonResult Structure(DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Cards.Structure().GetList(DB), loadOptions));
        }

        [HttpPost]
        public JsonResult UpdateStructure(CLiHR.Cards.Structure data)
        {
            if (data.Key == null)
            {
                CLiHR.Cards.Structure.Insert(DB, data);
            }
            else
            {
                CLiHR.Cards.Structure.Update(DB, data);
            }
            return Json(true);
        }

        [HttpDelete]
        public JsonResult DeleteStructure(Guid? Key)
        {
            int res = CLiHR.Cards.Structure.Delete(DB, Key);
            return Json(res);
        }
    }
}

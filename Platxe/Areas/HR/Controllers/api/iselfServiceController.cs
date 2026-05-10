using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.HR.Controllers.api
{
    [Area("HR")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/hr/[Controller]/[Action]")]
    public class iselfServiceController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly Guid? Subscribe;
        public iselfServiceController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
        }

        [HttpGet]
        public JsonResult RequestAdvance(int Status,Guid? Key,DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Request.Advance().GetList(DB, Status,Key), loadOptions));
        }
    }
}

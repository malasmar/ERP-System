using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.FixedAssets.Controllers.api
{
    [Area("FixedAssets")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/fxd/[Controller]/[Action]")]
    public class ireportsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly static object Locker = new object();
        public ireportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        [HttpGet]
        public JsonResult DetailsSheet(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.FixedAssets.Reports.FixtureSheet().DetailsSheet(DB,FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult SummarySheet(DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.FixedAssets.Reports.FixtureSheet().SummarySheet(DB, FirstDate, LastDate), loadOptions));
        }
    }
}

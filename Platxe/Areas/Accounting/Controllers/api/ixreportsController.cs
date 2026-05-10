using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Accounting.Controllers.api
{
    [Authorize]
    [Area("Accounting")]
    [Produces("application/json")]
    [Route("api/acc/[Controller]/[Action]")]
    public class ixreportsController : Controller
    {
        private readonly string DB;
        private readonly string xLan;
        private readonly static object Locker = new object();
        public ixreportsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }

        [HttpGet]
        public JsonResult TrialBalance(DateTime First, DateTime Last)
        {
            return Json(new CLiAccounting.Reports.TrialBalance().InteractiveTrial(DB, First, Last));
        }
        [HttpGet]
        public IActionResult NetIncomeTotal(DateTime FirstDate, DateTime LastDate)
        {
            return Json(CLiAccounting.core.NetIncome(DB, FirstDate,LastDate));
        }
        [HttpGet]
        public JsonResult ChartParentLevel()
        {
            return Json(new CLiAccounting.Reports.TreeBalance.ChartParentLevel().GetList(DB, "", xLan));
        }
    }
}

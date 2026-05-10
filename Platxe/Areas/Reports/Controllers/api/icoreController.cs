using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Reports.Controllers.api
{
    [Area("Reports")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Reports/[Controller]/[Action]")]
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
        public JsonResult ItemsSales(DateTime FirstDate,DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiSales.Reports.ItemsSales().GetList(DB,FirstDate,LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult ReturnItemsSales(DateTime FirstDate, DateTime LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiSales.Reports.ItemsSales().ReturnSales(DB, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult MonthlyNetSales(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiSales.Reports.MonthlySales().MonthlyNetSales(DB, Year), loadOptions));
        }
        [HttpGet]
        public JsonResult MonthlyQuantity(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiSales.Reports.MonthlySales().MonthlyQuantity(DB, Year), loadOptions));
        }
    }
}

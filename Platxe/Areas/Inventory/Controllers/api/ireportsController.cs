using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Areas.Inventory.Controllers.api
{
    [Area("Inventory")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/inv/[Controller]/[Action]")]
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
        public JsonResult StockStatment(Guid? Key,int Warehouse, DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Statments.StockStatment().GetList(DB,Key,Warehouse,FirstDate,LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult StockFullStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Statments.StockStatment().ReportWithoutWarehouse(DB, Key, FirstDate, LastDate), loadOptions));
        }
        [HttpGet]
        public JsonResult WarehouseBalance(int Warehouse, DateTime? Date, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Balance.WarehouseBalance().GetList(DB, Warehouse, Date), loadOptions));
        }
        [HttpGet]
        public JsonResult WarehousesBalance(DateTime? Date, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.Balance.WarehouseBalance().GetList(DB, Date), loadOptions));
        }

        //[HttpGet]
        //public JsonResult StockFullStatment(Guid? Key, DateTime? FirstDate, DateTime? LastDate, DataSourceLoadOptions loadOptions)
        //{
        //    return Json(DataSourceLoader.Load(new CLiInventory.Reports.Statments.StockStatment().ReportWithoutWarehouse(DB, Key, FirstDate, LastDate), loadOptions));
        //}

        [HttpGet]
        public JsonResult TransactionsDetails(int DocKind,int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiInventory.Reports.TransactionsDetails().GetList(DB, DocKind,Year), loadOptions));
        }

    }
}

using CLiCore;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Platxe.Areas.FixedAssets.Controllers.api
{
    [Area("FixedAssets")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/fxd/[Controller]/[Action]")]
    public class ioperationsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        private readonly static object Locker = new object();
        public ioperationsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        [HttpGet]
        public JsonResult BookValues(int Year, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiFinancial.FixedAssets.Operation.BookValues().GetList(DB), loadOptions));
        }
        [HttpGet]
        public JsonResult BookValuesDetails(Guid? Key)
        {
            return Json(new CLiFinancial.FixedAssets.Operation.BookValuesDetails().GetList(DB, xLan, Key));
        }

        [HttpPost]
        public JsonResult UpdateBookValues(CLiFinancial.FixedAssets.Operation.BookValues Header, string Data, bool isNew)
        {
            lock (Locker)
            {
                try
                {
                    OperationResult result;
                    var details = new List<CLiFinancial.FixedAssets.Operation.BookValuesDetails>();
                    details = JsonConvert.DeserializeObject<List<CLiFinancial.FixedAssets.Operation.BookValuesDetails>>(Data);
                    result = CLiFinancial.FixedAssets.Operation.core.UpdateBookValues(DB, Header, details, isNew);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    OperationResult result = new OperationResult();
                    result.Status = true;
                    result.Message = ex.Message;
                    return Json(result);
                }
            }
        }



    }
}

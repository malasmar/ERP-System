using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Platxe.Controllers.api
{
   
    [Produces("application/json")]
    [Route("api/[Controller]/[Action]")]
    [Authorize]
    public class iselectionsController : Controller
    {
        private readonly string DB;
        private readonly Guid? UserKey;
        private readonly string xLan;
        private readonly Guid? Subscribe;
        private readonly int Year;
        private readonly string UserEmail;
        public iselectionsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserKey = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("Key"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
            Subscribe = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("aiwSubscribe"));
            Year = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("aiwYear"));
            UserEmail = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }
        [HttpGet]
        public JsonResult Invoices(int Year, int DocumentKind, DataSourceLoadOptions loadOptions)
        {
            CLiInventory.Selections.Invoices cls = new CLiInventory.Selections.Invoices();
            return Json(DataSourceLoader.Load(cls.GetList(DB, DocumentKind), loadOptions));
        }

    }
}

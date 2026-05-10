using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Platxe.Areas.Payroll.Controllers.api
{
    [Area("Payroll")]
    [Authorize]
    [Produces("application/json")]
    [Route("api/Payroll/[Controller]/[Action]")]
    public class iSalariesController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        private readonly string xLan;
        public iSalariesController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
            xLan = httpContextAccessor.HttpContext.User.FindFirstValue("aiwLan");
        }
        [HttpGet]
        public JsonResult CreateSalaries(int Year,int Month,string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Salaries.CreateSalaries().GetList(DB,Year,Month, PaymentKind), loadOptions));
        }
        [HttpGet]
        public JsonResult  Attendance(int Year, int Month, string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Salaries.Attendance().GetList(DB, Year, Month, PaymentKind), loadOptions));
        }
        [HttpGet]
        public JsonResult Benefit(int Year, int Month, string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Salaries.Benefit().GetList(DB, Year, Month, PaymentKind), loadOptions));
        }
        [HttpGet]
        public JsonResult Deduction(int Year, int Month, string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Salaries.Deduction().GetList(DB, Year, Month, PaymentKind), loadOptions));
        }
        [HttpGet]
        public JsonResult SalariesSheet(int Year, int Month, string PaymentKind, DataSourceLoadOptions loadOptions)
        {
            return Json(DataSourceLoader.Load(new CLiHR.Salaries.SalariesSheet().GetList(DB, Year, Month, PaymentKind), loadOptions));
        }



        [HttpPost]
        public JsonResult CreateSalariesTable(int Year,int Month,string Values)
        {
            List<Guid> Keys = new List<Guid>();
            Keys = JsonConvert.DeserializeObject<List<Guid>>(Values);
            foreach (var key in Keys)
            {
                CLiHR.Salaries.core.CreateSalary(DB, key, Year, Month);
            }
            return Json(true);
        }
        [HttpPut]
        public JsonResult UpdateAttendance(Guid? key, string values)
        {
            CLiHR.Salaries.Attendance att;
            att = JsonConvert.DeserializeObject<CLiHR.Salaries.Attendance>(values);
            CLiHR.Salaries.core.UpdateAttendance(DB, att.Key, att.Absence, att.LateMinutes, att.Overtime);
            return Json(Ok());
        }
        [HttpPut]
        public JsonResult UpdateBenefit(Guid? key, string values)
        {
            CLiHR.Salaries.Benefit benfit;
            benfit = JsonConvert.DeserializeObject<CLiHR.Salaries.Benefit>(values);
            CLiHR.Salaries.core.UpdateBenefit(DB, benfit);
            return Json(Ok());
        }
        [HttpPut]
        public JsonResult UpdateDeduction(Guid? key, string values)
        {
            CLiHR.Salaries.Deduction deduction;
            deduction = JsonConvert.DeserializeObject<CLiHR.Salaries.Deduction>(values);
            CLiHR.Salaries.core.UpdateDeduction(DB, deduction);
            return Json(Ok());
        }
        [HttpPost]
        public JsonResult DeleteSalaries(string Values)
        {
            List<Guid> Keys = new List<Guid>();
            Keys = JsonConvert.DeserializeObject<List<Guid>>(Values);
            foreach (var key in Keys)
            {
                CLiHR.Salaries.core.DeleteSalary(DB, key);
            }
            return Json(true);
        }
    }
}

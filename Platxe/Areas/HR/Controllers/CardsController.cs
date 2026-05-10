using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Platxe.Areas.HR.Controllers
{
    [Area("HR")]
    [Route("HR/[Controller]/[Action]")]
	[Authorize(Roles = "OMEs-HR")]
	[Authorize(Roles = "PL-HR")]
	public class CardsController : Controller
    {
        private readonly string DB;
        private readonly int UserID;
        public CardsController(IHttpContextAccessor httpContextAccessor)
        {
            DB = httpContextAccessor.HttpContext.User.FindFirstValue("aiwDB");
            UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue("uID"));
        }
        public IActionResult Employee()
        {
            return View();
        }
        public IActionResult EmployeeDetails(Guid? Key)
        {
            return PartialView("_Employee", new CLiHR.Cards.Employee().GetItem(DB, Key));
        }
        public IActionResult Department()
        {
            return View();
        }
        public IActionResult DepartmentDetails(Guid? Key)
        {
            return PartialView("_Department", new CLiHR.Cards.Department().GetItem(DB, Key));
        }

        public IActionResult BankNames()
        {
            return View();
        }
        public IActionResult BankNamesDetails(Guid? Key)
        {
            return PartialView("_BankNames", new CLiHR.Cards.BankNames().GetItem(DB, Key));
        }

        public IActionResult JobTitle()
        {
            return View();
        }
        public IActionResult JobTitleDetails(Guid? Key)
        {
            return PartialView("_JobTitle", new CLiHR.Cards.JobTitle().GetItem(DB, Key));
        }
        public IActionResult Biometric()
        {
            return View();
        }
        public IActionResult BiometricDetails(Guid? Key)
        {
            return PartialView("_Biometric", new CLiHR.Cards.Biometric().GetItem(DB, Key));
        }
        public IActionResult Shift(string Key = "")
        {
            return View(new CLiHR.Cards.Shift().GetItem(DB,Key));
        }
        public IActionResult ShiftList()
        {
            return View();
        }
        public IActionResult Location()
        {
            return View();
        }
        public IActionResult LocationDetails(Guid? Key)
        {
            return PartialView("_Location", new CLiHR.Cards.Location().GetItem(DB, Key));
        }
    }
}

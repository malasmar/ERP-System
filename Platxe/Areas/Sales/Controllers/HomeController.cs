using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Sales.Controllers
{
    [Area("Sales")]
	[Authorize(Roles = "OMEs-Sales")]
	[Authorize(Roles = "PL-Sales")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

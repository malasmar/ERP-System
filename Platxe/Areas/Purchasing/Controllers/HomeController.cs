using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
namespace Platxe.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
	[Authorize(Roles = "OMEs-Purchasing")]
	[Authorize(Roles = "PL-Purchasing")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Platxe.Areas.FixedAssets.Controllers
{
    [Area("FixedAssets")]
	[Authorize(Roles = "OMEs-FixedAssets")]
	[Authorize(Roles = "PL-FixedAssets")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

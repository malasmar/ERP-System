using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Platxe.Controllers
{
    [Authorize]
    public class errorsController : Controller
    {
        [Route("errors/404")]
        public IActionResult error404()
        {
            return View();
        }
        [Route("errors/{code}")]
        public IActionResult error(int code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}

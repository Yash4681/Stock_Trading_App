using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksAppWithXUnit.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if(exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                ViewBag.Error = exceptionHandlerPathFeature.Error.Message;
            }

            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace MyAppWeb.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

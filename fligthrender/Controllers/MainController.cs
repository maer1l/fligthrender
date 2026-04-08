using Microsoft.AspNetCore.Mvc;

namespace fligthrender.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

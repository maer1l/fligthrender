using Microsoft.AspNetCore.Mvc;

namespace fligthrender.Controllers
{
    public class FlightsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

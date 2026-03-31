using fligthrender.Data;
using fligthrender.Models;
using fligthrender.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using System.Numerics;

namespace fligthrender.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlightrenderContext _context;

        public HomeController(FlightrenderContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            

            int count = _context.Planes.Count();
            if (count > 0)
            {
                var random = new Random();
                int index = random.Next(count);

                var randomPlane = _context.Planes
                    .Include(p => p.Brand)
                    .Skip(index)
                    .FirstOrDefault();

                var vmodel = new PlaneWithPhoto { plane = randomPlane };


                var paths = _context.Planespictures
                                    .Where(p => p.PlaneId == randomPlane.PlaneId)
                                    .Select(p => p.Path)
                                    .ToList();


                vmodel.paths.AddRange(paths);

                return View(vmodel);
            }
            else
            {
                return View();
            }

            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

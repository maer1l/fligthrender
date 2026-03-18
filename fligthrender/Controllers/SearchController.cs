using fligthrender.Data;
using fligthrender.Models;
using fligthrender.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace fligthrender.Controllers
{
    public class SearchController : Controller
    {
        private readonly FlightrenderContext _context;

        public SearchController(FlightrenderContext context)
        {
            _context = context;
        }
        public IActionResult Index(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new SearchPlanesManufacturer());
            }

            SearchPlanesManufacturer result = new SearchPlanesManufacturer();

            var planes = _context.Planes
             .Include(p => p.Brand)
             .Where(c => c.Model.Contains(query)
             || c.Brand.Name.Contains(query)
             || c.Year.ToString().Contains(query))
             .ToList();

            
            result.Planes.AddRange(planes);

            var manufacturer = _context.Manufacturers
            .Where(m => m.Name.Contains(query)
            || m.Description.Contains(query)
            || m.Address.Contains(query))
            .ToList();


            result.Manufacturers.AddRange(manufacturer);

            return View(result);
        }
    }
}

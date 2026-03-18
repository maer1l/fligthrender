using fligthrender.Data;
using fligthrender.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace fligthrender.Controllers
{
    public class PlanesController : Controller
    {
        private readonly FlightrenderContext _context;

        public PlanesController(FlightrenderContext context)
        {
            _context = context;
        }

        // GET: Planes
        public async Task<IActionResult> Index()
        {
            var flightrenderContext = _context.Planes.Include(p => p.Brand);
            return View(await flightrenderContext.ToListAsync());
        }

        // GET: Planes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = await _context.Planes
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.PlaneId == id);
            if (plane == null)
            {
                return NotFound();
            }

            return View(plane);
        }

        // GET: Planes/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Manufacturers, "BrandId", "Name");
            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaneId,Model,Speed,Weight,Price,Year,BrandId")] Plane plane)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Manufacturers, "BrandId", "Name", plane.BrandId);
            return View(plane);
        }

        // GET: Planes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = await _context.Planes.FindAsync(id);
            if (plane == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Manufacturers, "BrandId", "Name", plane.BrandId);
            return View(plane);
        }

        // POST: Planes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaneId,Model,Speed,Weight,Price,Year,BrandId")] Plane plane)
        {
            if (id != plane.PlaneId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaneExists(plane.PlaneId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Manufacturers, "BrandId", "Name", plane.BrandId);
            return View(plane);
        }

        // GET: Planes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plane = await _context.Planes
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.PlaneId == id);
            if (plane == null)
            {
                return NotFound();
            }

            return View(plane);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plane = await _context.Planes.FindAsync(id);
            if (plane != null)
            {
                _context.Planes.Remove(plane);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaneExists(int id)
        {
            return _context.Planes.Any(e => e.PlaneId == id);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> SearchWithAjax(string request)
        {
            IEnumerable<Plane> filteredplanes = null;
            var planes = await _context.Planes.ToListAsync();
            var brands = await _context.Manufacturers.ToListAsync();
            if (!request.IsNullOrEmpty())
            {
                request = request.Trim();
                decimal o = 0;
                if(decimal.TryParse(request, out o))
                {
                    filteredplanes = from p in planes where p.Price == o select p;
                    filteredplanes = filteredplanes.Union(from p in planes where p.Weight == decimal.ToDouble(o) select p);
                    filteredplanes = filteredplanes.Union(from p in planes where p.Speed == decimal.ToInt32(o) select p);
                    filteredplanes = filteredplanes.Union(from p in planes where p.Speed == decimal.ToInt32(o) select p);
                    filteredplanes = filteredplanes.Union(from p in planes where p.Year == decimal.ToInt32(o) select p);
                    filteredplanes = filteredplanes.Union(from p in planes where p.Model == request select p);
                }
                else
                {
                    filteredplanes = from p in planes where p.Model == request select p;
                    try
                    {
                        int id = brands.SingleOrDefault(m => m.Name == request).BrandId;
                        filteredplanes = filteredplanes.Union(from p in planes where p.BrandId == id select p);
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                filteredplanes = planes;
            }

            return PartialView(filteredplanes);
        }

        [Route("Planes/price/{price}")]
        public async Task<IActionResult> PlanesByPrice(decimal price)
        {
            var planes = await _context.Planes.Include(p => p.Brand).ToListAsync();
            var filteredplanes = from p in planes where p.Price <= price select p;
            return View("Index", filteredplanes);
        }

        [Route("Planes/brand/{Brand}")]
        public async Task<IActionResult> PlanesByBrand(string Brand)
        {
            var brands = await _context.Manufacturers.ToListAsync();
            var planes = await _context.Planes.ToListAsync();
            int id = brands.SingleOrDefault(m => m.Name == Brand).BrandId;
            var filteredplanes = from p in planes where p.BrandId == id select p;
            return View("Index", filteredplanes);
        }
    }
}

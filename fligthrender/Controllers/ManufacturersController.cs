using fligthrender.Data;
using fligthrender.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fligthrender.Controllers
{
    public class ManufacturersController : Controller
    {
        private readonly FlightrenderContext _context;

        public ManufacturersController(FlightrenderContext context)
        {
            _context = context;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Manufacturers.ToListAsync());
        }

        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,Name,Description,Address")] Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manufacturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,Name,Description,Address")] Manufacturer manufacturer)
        {
            if (id != manufacturer.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturer.BrandId))
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
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultParam = new SqlParameter
            {
                ParameterName = "@Result",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            var manufacturer = await _context.Manufacturers.FindAsync(id);
            await _context.Database.ExecuteSqlRawAsync("EXEC CheckManufacturer @brand_id = {0}, @Result = @Result OUTPUT", id, resultParam);

            int result = (int)resultParam.Value;

            if (result == 0)
            {
                TempData["AlertMessage"] = "Нельзя удалить бренд, у него есть связанные самолеты!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturerExists(int id)
        {
            return _context.Manufacturers.Any(e => e.BrandId == id);
        }

        [Route("Manufacturers/name/{name}")]
        public ActionResult ManufacturerByName(string name)
        {
            var filtered = _context.Manufacturers
            .Include(m => m.Planes)
            .Where(p => p.Name.Contains(name))
            .ToList();

            return View("Index", filtered);
        }

        [Route("Manufacturers/description/{description}")]
        public ActionResult ManufacturerByDescription(string description)
        {
            var filtered = _context.Manufacturers
            .Include(m => m.Planes)
            .Where(p => p.Description.Contains(description))
            .ToList();

            return View("Index", filtered);
        }

        [Route("Manufacturers/address/{address}")]
        public ActionResult ManufacturerByAddress(string address)
        {
            var filtered = _context.Manufacturers
            .Include(m => m.Planes)
            .Where(p => p.Address.Contains(address))
            .ToList();

            return View("Index", filtered);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> SearchWithAjax(string request)
        {
            IEnumerable<Manufacturer> filteredmanufacturers = null;
            var brands = await _context.Manufacturers.ToListAsync();
            if (!request.IsNullOrEmpty())
            {
                request = request.Trim();

                filteredmanufacturers = from p in brands where p.Name == request select p;
                filteredmanufacturers = filteredmanufacturers.Union(from p in brands where p.Description == request select p);
                filteredmanufacturers = filteredmanufacturers.Union(from p in brands where p.Address == request select p);
            }
            else
            {
                filteredmanufacturers = brands;
            }

            return PartialView(filteredmanufacturers);
        }
    }
}

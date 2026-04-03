using fligthrender.Data;
using fligthrender.Models;
using fligthrender.ViewModels;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace fligthrender.Controllers
{
    public class PlanesController : Controller
    {
        private readonly FlightrenderContext _context;

        private readonly ILogger<HomeController> _logger;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public PlanesController(FlightrenderContext context, ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
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

            if (id == null)
            {
                return NotFound();
            }

            var vmodel = new PlaneWithPhoto { plane = plane };


            var paths = _context.Planespictures
                                .Where(p => p.PlaneId == id)
                                .Select(p => p.Path)
                                .ToList();


            vmodel.paths.AddRange(paths);

            return View(vmodel);
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

        // валидация JQuery Ajax
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Save2([FromBody] Plane plane)
        {
            // если форма не валидна
            if (!ModelState.IsValid)
            {
                var errorModel =
                        from x in ModelState.Keys
                        where ModelState[x].Errors.Count > 0
                        select new
                        {
                            key = x,
                            errors = ModelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                        };

                return Json(new { success = false, errors = errorModel });
            }
            else
            {
                _context.Add(plane);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Validation successfull" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadFiles(PlaneWithPhoto model)
        {
            // перебрать коллекцию полученных файлов
            foreach (IFormFile file in model.files)
            {
                // проверка наличия файла
                if (file != null)
                {
                    var savePath = _hostingEnvironment.WebRootPath + "/Images/" + file.FileName;

                    // сохранение файла
                    using (var fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    await _context.Database.ExecuteSqlRawAsync($"EXEC nProc @PATH, @PLANEID", new SqlParameter("@PATH", "/Images/" + file.FileName), new SqlParameter("@PLANEID", model.plane.PlaneId));

                    // установка сообщения о загрузке файлов
                    ViewBag.UploadStatus = model.files.Count().ToString() + " files uploaded successfully.";
                }
            }

            var paths = _context.Planespictures
                                .Where(p => p.PlaneId == model.plane.PlaneId)
                                .Select(p => p.Path)
                                .ToList();

            var result = Json(paths, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });

            return result;
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
            var vmodel = new PlaneWithPhoto { plane = plane };

            
            var paths = _context.Planespictures
                                .Where(p => p.PlaneId == id)
                                .Select(p => p.Path)
                                .ToList();

            
            vmodel.paths.AddRange(paths);

            return View(vmodel);
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
            try
            {
                await _context.Database.ExecuteSqlRawAsync($"EXEC deletingPlane @PLANEID", new SqlParameter("@PLANEID", id));

                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex)
            {
                return RedirectToAction(nameof(Delete), new { id });
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePhoto(string photoPath, int planeId)
        {
            var photo = _context.Planespictures
                                .FirstOrDefault(p => p.PlaneId == planeId && p.Path == photoPath);

            if (photo != null)
            {
                _context.Planespictures.Remove(photo);
                _context.SaveChanges();
            }

            return Json(new { success = true, photoPath });
        }
    }
}

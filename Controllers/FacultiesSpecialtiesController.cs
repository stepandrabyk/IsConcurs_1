using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IscConcursLr1;

namespace IscConcursLr1.Controllers
{
    public class FacultiesSpecialtiesController : Controller
    {
        private readonly DBIsConcursContext _context;

        public FacultiesSpecialtiesController(DBIsConcursContext context)
        {
            _context = context;
        }

        // GET: FacultiesSpecialties
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.FacultiesId = id;
            ViewBag.FacultiesName = name;
            
            
            var specialtiesInFaculties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == id).Include(c => c.FsFacultiesNavigation).Include(c => c.FsSpecialtiesNavigation);
         
           return View(await specialtiesInFaculties.ToListAsync());
        }

        // GET: FacultiesSpecialties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultiesSpecialties = await _context.FacultiesSpecialties
                .Include(f => f.FsFacultiesNavigation)
                .Include(f => f.FsSpecialtiesNavigation)
                .FirstOrDefaultAsync(m => m.FacultiesSpecialtiesId == id);
            if (facultiesSpecialties == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Statements", new { id = facultiesSpecialties.FacultiesSpecialtiesId });
        }

        // GET: FacultiesSpecialties/Create
        public IActionResult Create(int fsFaculties)
        {
            ViewBag.FacultiesId = fsFaculties;
           ViewData["FsSpecialties"] = new SelectList(_context.Specialties, "SpecialtiesId", "Name");
            return View();
        }

        // POST: FacultiesSpecialties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int fsFaculties, [Bind("FacultiesSpecialtiesId,FsFaculties,FsSpecialties,Budget,Contract")] FacultiesSpecialties facultiesSpecialties)
        {
            facultiesSpecialties.FsFaculties = fsFaculties;
            if (ModelState.IsValid)
            {
                _context.FacultiesSpecialties.Add(facultiesSpecialties);
                await _context.SaveChangesAsync();
                var specialtiesinFaculties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == fsFaculties).Include(c => c.FsFacultiesNavigation).Include(c => c.FsSpecialtiesNavigation);
                return RedirectToAction("Index", "FacultiesSpecialties", new { id = fsFaculties, specialtiesinFaculties });
            }
           var specialtiesInFaculties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == fsFaculties).Include(c => c.FsFacultiesNavigation).Include(c => c.FsSpecialtiesNavigation);

            return RedirectToAction("Index", "FacultiesSpecialties", new { id = fsFaculties, specialtiesInFaculties });
        }

        // GET: FacultiesSpecialties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultiesSpecialties = await _context.FacultiesSpecialties.FindAsync(id);
            if (facultiesSpecialties == null)
            {
                return NotFound();
            }
            ViewData["FsFaculties"] = new SelectList(_context.Faculties, "FacultiesId", "Name", facultiesSpecialties.FsFaculties);
            ViewData["FsSpecialties"] = new SelectList(_context.Specialties, "SpecialtiesId", "Name", facultiesSpecialties.FsSpecialties);
            return View(facultiesSpecialties);
        }

        // POST: FacultiesSpecialties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultiesSpecialtiesId,FsFaculties,FsSpecialties,Budget,Contract")] FacultiesSpecialties facultiesSpecialties)
        {
            if (id != facultiesSpecialties.FacultiesSpecialtiesId)
            {
                return NotFound();
            }
            int fsFaculties = id;
            var specialtiesInFaculties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == fsFaculties).Include(c => c.FsFacultiesNavigation).Include(c => c.FsSpecialtiesNavigation);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facultiesSpecialties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultiesSpecialtiesExists(facultiesSpecialties.FacultiesSpecialtiesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
                return RedirectToAction("Index", "FacultiesSpecialties", new { id = fsFaculties, specialtiesInFaculties });
            }
             return RedirectToAction("Index", "FacultiesSpecialties", new { id = fsFaculties, specialtiesInFaculties });
           
        }

        // GET: FacultiesSpecialties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultiesSpecialties = await _context.FacultiesSpecialties
                .Include(f => f.FsFacultiesNavigation)
                .Include(f => f.FsSpecialtiesNavigation)
                .FirstOrDefaultAsync(m => m.FacultiesSpecialtiesId == id);
            if (facultiesSpecialties == null)
            {
                return NotFound();
            }

            return View(facultiesSpecialties);
        }

        // POST: FacultiesSpecialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facultiesSpecialties = await _context.FacultiesSpecialties.FindAsync(id);
            int FacultyId = facultiesSpecialties.FsFaculties;
            var statements = _context.Statements.Where(c => c.StFacultiesSpecialties == id);
            foreach (var c in statements)
                _context.Statements.Remove(c);
            _context.FacultiesSpecialties.Remove(facultiesSpecialties);
            await _context.SaveChangesAsync();
            var specialtiesInFaculties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == FacultyId).Include(c => c.FsFacultiesNavigation).Include(c => c.FsSpecialtiesNavigation);

            return RedirectToAction("Index", "FacultiesSpecialties", new { id = FacultyId });
           
        }

        private bool FacultiesSpecialtiesExists(int id)
        {
            return _context.FacultiesSpecialties.Any(e => e.FacultiesSpecialtiesId == id);
        }
    }
}

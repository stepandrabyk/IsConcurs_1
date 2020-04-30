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
    public class FacultiesController : Controller
    {
        private readonly DBIsConcursContext _context;

        public FacultiesController(DBIsConcursContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.UniversityId = id;
            ViewBag.UniversityName = name;
            var FacultiesinUniver = _context.Faculties.Where(c => c.FacultiesUniversity == id).Include(c => c.FacultiesUniversityNavigation);
            return View(await FacultiesinUniver.ToListAsync());
        }
        public IActionResult CorrectName(string name)
        {
            var faculty = _context.Faculties.Where(c => c.Name == name).FirstOrDefault();
            if(faculty != null)
            {
                return Json($"Імя {name} вже використовується.");
            }
            return Json(true);
        }
        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties
                .Include(f => f.FacultiesUniversityNavigation)
                .FirstOrDefaultAsync(m => m.FacultiesId == id);
            if (faculties == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "FacultiesSpecialties", new { id = faculties.FacultiesId, name = faculties.Name });
        }

        // GET: Faculties/Create
        public IActionResult Create(int universityId)
        {
            ViewBag.UniversityId = universityId;
            ViewBag.UniversityName = _context.Universities.Where(c => c.UniversityId == universityId).FirstOrDefault().Name;
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int universityId,[Bind("FacultiesId,Name,FacultiesUniversity")] Faculties faculties)
        {
            faculties.FacultiesUniversity = universityId;

            if (ModelState.IsValid)
            {
                _context.Add(faculties);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Faculties", new { id = universityId, _context.Faculties.Where(c => c.FacultiesUniversity == universityId).FirstOrDefault().Name });
            }

            return RedirectToAction("Index", "Faculties", new { id = universityId, _context.Faculties.Where(c=>c.FacultiesUniversity == universityId).FirstOrDefault().Name});

        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties.FindAsync(id);
            if (faculties == null)
            {
                return NotFound();
            }

            ViewData["FacultiesUniversity"] = new SelectList(_context.Universities, "UniversityId", "Name", faculties.FacultiesUniversity);
            return View(faculties);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacultiesId,Name,FacultiesUniversity")] Faculties faculties)
        {
            string name = faculties.Name;
            int facultyId = faculties.FacultiesId;
            faculties = await _context.Faculties.FindAsync(id);
            if (id != facultyId)
            {
                return NotFound();
            }
            faculties.Name = name;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Faculties.Update(faculties);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultiesExists(faculties.FacultiesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Faculties", new { id = faculties.FacultiesUniversity, _context.Faculties.Where(c => c.FacultiesUniversity == faculties.FacultiesUniversity).FirstOrDefault().Name });
            }


            return RedirectToAction("Index", "Faculties", new { id = faculties.FacultiesUniversity, _context.Faculties.Where(c => c.FacultiesUniversity == faculties.FacultiesUniversity).FirstOrDefault().Name });
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete( int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculties = await _context.Faculties
                .Include(f => f.FacultiesUniversityNavigation)
                .FirstOrDefaultAsync(m => m.FacultiesId == id);
            if (faculties == null)
            {
                return NotFound();
            }

            return View(faculties);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculties = await _context.Faculties.FindAsync(id);
            int UniversityId = faculties.FacultiesUniversity;
            var specialtiesInFaculty = _context.FacultiesSpecialties.Where(c => c.FsFaculties == id);
            foreach (var spec in specialtiesInFaculty)
            {

                var statement = _context.Statements.Where(c => c.StFacultiesSpecialties == spec.FacultiesSpecialtiesId);
                foreach(var c in statement)
                {
                    _context.Statements.Remove(c);
                }
                    _context.FacultiesSpecialties.Remove(spec);
            }
            _context.Faculties.Remove(faculties);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Faculties", new { id = UniversityId}); 
        }

        private bool FacultiesExists(int id)
        {
            return _context.Faculties.Any(e => e.FacultiesId == id);
        }
    }
}

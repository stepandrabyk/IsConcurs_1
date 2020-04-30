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
    public class StatementsController : Controller
    {
        private readonly DBIsConcursContext _context;

        public StatementsController(DBIsConcursContext context)
        {
            _context = context;
        }
        // GET: Statements
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.FacultiesSpecialtiesId = id;
            var statements = _context.Statements.Where(c => c.StFacultiesSpecialties == id).
                Include(c => c.StStudentNavigation).OrderByDescending(c => c.StStudentNavigation.Mark);
       
            return View(await statements.ToListAsync());
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult UniqueStatement(int StStudent, int StFacultiesSpecialties)
        {
            
            var statement = _context.Statements.Where(c => c.StFacultiesSpecialties == StFacultiesSpecialties).Where(c => c.StStudent == StStudent).FirstOrDefault();
            
            if (statement != null)
            {
                return Json($"Заява вже існує.");
            }

            return Json(true);
        }
        // GET: Statements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var statements = await _context.Statements
                .Include(s => s.StFacultiesSpecialtiesNavigation)
                .Include(s => s.StStudentNavigation)
                .FirstOrDefaultAsync(m => m.StatementId == id);
            if (statements == null)
            {
                return NotFound();
            }
            return View(statements);
        }

        // GET: Statements/Create
        public IActionResult Create(int facultiesSpecialtiesId)
        {
            ViewBag.FacultiesSpecialtiesId = facultiesSpecialtiesId;
          ViewData["StStudent"] = new SelectList(_context.Students, "StudentId", "Name");
            return View();
        }

        // POST: Statements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int facultiesSpecialtiesId, [Bind("StatementId,StStudent,StFacultiesSpecialties")] Statements statements)
        {
            statements.StFacultiesSpecialties = facultiesSpecialtiesId;
            var statementFS = _context.Statements.Where(c => c.StFacultiesSpecialties == facultiesSpecialtiesId).Include(c => c.StStudentNavigation);
            if (ModelState.IsValid)
            {
                
                    _context.Statements.Add(statements);
          

                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Statements", new { id = facultiesSpecialtiesId, statementFS});
            }
            return RedirectToAction("Index", "Statements", new { id = facultiesSpecialtiesId, statementFS });
        }

        // GET: Statements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statements = await _context.Statements.FindAsync(id);
            
            if (statements == null)
            {
                return NotFound();
            }
            ViewData["StStudent"] = new SelectList(_context.Students, "StudentId", "Name", statements.StStudent);
            return View(statements);
        }

        // POST: Statements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatementId,StStudent,StFacultiesSpecialties")] Statements statements)
        {
            int student = statements.StStudent;
            statements = await _context.Statements.FindAsync(id);
            
            if (id != statements.StatementId)
            {
                return NotFound();
            }
            statements.StStudent = student;
            int facultiesSpecialtiesId = statements.StFacultiesSpecialties;
            var statementFS = _context.Statements.Where(c => c.StFacultiesSpecialties == facultiesSpecialtiesId).Include(c => c.StStudentNavigation);
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Statements.Update(statements);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatementsExists(statements.StatementId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Statements",new {id = facultiesSpecialtiesId, statementFS });
            }
            
            return RedirectToAction("Index", "Statements", new { id = facultiesSpecialtiesId, statementFS });
        }

        // GET: Statements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statements = await _context.Statements
                .Include(s => s.StFacultiesSpecialtiesNavigation)
                .Include(s => s.StStudentNavigation)
                .FirstOrDefaultAsync(m => m.StatementId == id);
            if (statements == null)
            {
                return NotFound();
            }

            return View(statements);
        }

        // POST: Statements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var statements = await _context.Statements.FindAsync(id);
            int facultiesSpecialtiesId = statements.StFacultiesSpecialties;
            var statementFS = _context.Statements.Where(c => c.StFacultiesSpecialties == facultiesSpecialtiesId).Include(c => c.StStudentNavigation);
            _context.Statements.Remove(statements);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Statements", new {id = facultiesSpecialtiesId, statementFS });
        }

        private bool StatementsExists(int id)
        {
            return _context.Statements.Any(e => e.StatementId == id);
        }
    }
}

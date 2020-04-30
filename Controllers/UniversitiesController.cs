using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IscConcursLr1;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace IscConcursLr1.Controllers
{
    
    public class UniversitiesController : Controller
    {
        private readonly DBIsConcursContext _context;

        public UniversitiesController(DBIsConcursContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            foreach (var c in _context.Universities)
            {
                ViewBag.UniversityId = c.UniversityId;
            }
            return View(await _context.Universities.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var universities = await _context.Universities
                .FirstOrDefaultAsync(m => m.UniversityId == id);
            if (universities == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Faculties", new { id = universities.UniversityId, name = universities.Name });
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult CorrectName(string name)
        {
            var university = _context.Universities.Where(c => c.Name ==name).FirstOrDefault();
            if (university!=null)
            {
                return Json($"Імя {name} вже використовується.");
            }

            return Json(true);
        }
    
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniversityId,Name,Info")] Universities universities)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(universities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
         
            return View(universities);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var universities = await _context.Universities.FindAsync(id);
            if (universities == null)
            {
                return NotFound();
            }
            return View(universities);
        }

        public ActionResult Export()
        {
            using (XLWorkbook book = new XLWorkbook())
            {
                var universities = _context.Universities.Include(c => c.Faculties).ToList();
                
                foreach (var univ in universities)
                {
                    int num = 0;
                    var vorksheet = book.Worksheets.Add(univ.Name);
                    vorksheet.Cell("A1").Value = "Факультет";
                    vorksheet.Cell("B1").Value = "Спецальність";
                    vorksheet.Cell("C1").Value = "Студент";
                    vorksheet.Cell("D1").Value = "Оцінка";
                    var faculties = univ.Faculties.ToList();
                 
                    for(int i=0;i<faculties.Count;i++)
                    {
                        
                        var facultyspecialties = _context.FacultiesSpecialties.Where(c => c.FsFaculties == faculties[i].FacultiesId).ToList();
                        for(int j=0;j<facultyspecialties.Count;j++)
                        {
                            var statements = _context.Statements.Where(c => c.StFacultiesSpecialties == facultyspecialties[j].FacultiesSpecialtiesId).ToList();
                            for(int k=0;k<statements.Count;k++)
                            {
                                
                                vorksheet.Cell(num + 2, 1).Value = faculties[i].Name;
                                vorksheet.Cell(num + 2, 2).Value = _context.Specialties.Where(c => c.SpecialtiesId == facultyspecialties[j].FsSpecialties).FirstOrDefault().Name;
                                vorksheet.Cell(num + 2, 3).Value =  _context.Students.Where(c=>c.StudentId == statements[k].StStudent).FirstOrDefault().Name;
                                vorksheet.Cell(num + 2, 4).Value = _context.Students.Where(c => c.StudentId == statements[k].StStudent).FirstOrDefault().Mark;
                                num++;
                            }
                        }
                         
                    }

                }
                using (var stream = new MemoryStream())
                {
                    book.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"IS_Concurs_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if(ModelState.IsValid)
            {
                if(fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook book = new XLWorkbook(stream,XLEventTracking.Disabled))
                        {
                            foreach (var worksheet in book.Worksheets)
                            {
                                Universities university;
                                var universities = _context.Universities.Where(c => c.Name == worksheet.Name).ToList();
                                if (universities.Count >= 1)
                                {
                                    university = universities[0];
                                }
                                else
                                {
                                    university = new Universities();
                                    university.Name = worksheet.Name;
                                    _context.Universities.Add(university);
                                    await _context.SaveChangesAsync();
                                }  
                                foreach ( IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    Faculties faculty;
                                    string facultyName = row.Cell(1).Value.ToString();
                                    var faculties = _context.Faculties.Where(c => c.Name == facultyName).ToList();
                                    if(faculties.Count>=1)
                                    {
                                        faculty = faculties[0];
                                    }
                                    else
                                    {
                                        faculty = new Faculties();
                                        faculty.Name = facultyName;
                                        faculty.FacultiesUniversity = university.UniversityId;
                                        _context.Faculties.Add(faculty);
                                        await _context.SaveChangesAsync();
                                    }
                                    Specialties specialty;
                                    string specialityName = row.Cell(2).Value.ToString();
                                    var specialties = _context.Specialties.Where(c => c.Name == specialityName).ToList();
                                    if(specialties.Count>=1)
                                    {
                                        specialty = specialties[0];
                                    }
                                    else
                                    {
                                        specialty = new Specialties();
                                        specialty.Name = specialityName;
                                        _context.Specialties.Add(specialty);
                                        await _context.SaveChangesAsync();
                                    }
                                    FacultiesSpecialties facultiesSpecialty;
                                    var facultyspecialties = _context.FacultiesSpecialties.Where(c => c.FsSpecialties == specialty.SpecialtiesId).Where(c => c.FsFaculties == faculty.FacultiesId).ToList();
                                    if(facultyspecialties.Count>=1)
                                    {
                                        facultiesSpecialty = facultyspecialties[0];
                                    }
                                    else
                                    {
                                        facultiesSpecialty = new FacultiesSpecialties();
                                        facultiesSpecialty.FsFaculties = faculty.FacultiesId;
                                        facultiesSpecialty.FsSpecialties = specialty.SpecialtiesId;
                                        _context.FacultiesSpecialties.Add(facultiesSpecialty);
                                        await _context.SaveChangesAsync();
                                    }
                                    Students student;
                                    string studentsName = row.Cell(3).Value.ToString();
                                    int studentsMark = Convert.ToInt32(row.Cell(4).Value.ToString());
                                    var students = _context.Students.Where(c => c.Name == studentsName).ToList();
                                    if(students.Count>=1)
                                    {
                                        student = students[0];
                                        if(student.Mark!= studentsMark)
                                        {
                                            student.Mark = studentsMark;
                                            _context.Students.Update(student);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                    else
                                    {
                                        student = new Students();
                                        student.Name = studentsName;
                                        student.Mark = studentsMark;
                                        _context.Students.Add(student);
                                        await _context.SaveChangesAsync();
                                    }
                                    Statements statement;
                                    var statements = _context.Statements.Where(c => c.StStudent == student.StudentId).Where(c => c.StFacultiesSpecialties == facultiesSpecialty.FacultiesSpecialtiesId).ToList();
                                    if(statements.Count>=1)
                                    {
                                        statement = statements[0];
                                    }
                                    else
                                    {
                                        statement = new Statements();
                                        statement.StStudent = student.StudentId;
                                        statement.StFacultiesSpecialties = facultiesSpecialty.FacultiesSpecialtiesId;
                                        _context.Statements.Add(statement);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UniversityId,Name,Info")] Universities universities)
        {
            if (id != universities.UniversityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(universities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversitiesExists(universities.UniversityId))
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
            return View(universities);
        }

        // GET: Universities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var universities = await _context.Universities
                .FirstOrDefaultAsync(m => m.UniversityId == id);
            if (universities == null)
            {
                return NotFound();
            }

            return View(universities);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var universities = await _context.Universities.FindAsync(id);
            var facultiesInUniversity = _context.Faculties.Where(c => c.FacultiesUniversity == id);
            foreach(var faculty in facultiesInUniversity)
            {
                _context.Faculties.Remove(faculty);
               
            }
            _context.Universities.Remove(universities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniversitiesExists(int id)
        {
            return _context.Universities.Any(e => e.UniversityId == id);
        }
    }
}

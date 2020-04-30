using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IscConcursLr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PieChartController : ControllerBase
    {
        private readonly DBIsConcursContext _context;
        public PieChartController(DBIsConcursContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var universities = _context.Universities.Include(c => c.Faculties).ToList();
            List<object> facultiesUniversity = new List<object>();
            facultiesUniversity.Add(new[] { "Університет", "Факультети" });
            foreach(var c in universities)
            {
                facultiesUniversity.Add(new object [] { c.Name, c.Faculties.Count() });
            }
            return new JsonResult(facultiesUniversity);
        }
        public JsonResult JsonData1()
        {
            var faculties = _context.Faculties.Include(c => c.FacultiesSpecialties).ToList();
            List<object> facultiesUniversity = new List<object>();
            facultiesUniversity.Add(new[] { "Факультети", "Спеціальності" });
            foreach (var c in faculties)
            {
                facultiesUniversity.Add(new object[] { c.Name, c.FacultiesSpecialties.Count() });
            }
            return new JsonResult(facultiesUniversity);
        }
    }
}
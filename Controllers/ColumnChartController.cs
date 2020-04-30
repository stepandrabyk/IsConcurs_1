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
    public class ColumnChartController : ControllerBase
    {
        private readonly DBIsConcursContext _context;
        public ColumnChartController(DBIsConcursContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var students = _context.Students.Include(c => c.Statements).ToList();
            List<object> facultiesUniversity = new List<object>();
            facultiesUniversity.Add(new[] { "Студенти", "Заяви" });
            foreach (var c in students)
            {
                facultiesUniversity.Add(new object[] { c.Name, c.Statements.Count() });
            }
            return new JsonResult(facultiesUniversity);
        }
    }
}
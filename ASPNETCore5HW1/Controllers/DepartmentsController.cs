using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCore5HW1.Models;
using Omu.ValueInjecter;

namespace ASPNETCore5HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public DepartmentsController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await db.Departments.ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await db.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department departmentVM) {
            if (!DepartmentExists(id)) {
                return NotFound();
            }

            Department department = await db.Departments.FindAsync(id);
            department?.InjectFrom(departmentVM);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<Department>> PostDepartment(Department departmentVM) {
            Department department = new Department();
            department.InjectFrom(departmentVM);

            var entry = db.Departments.Add(department);
            await db.SaveChangesAsync();

            department =(Department) entry.GetDatabaseValues().ToObject();
            return Created($"/api/Departments/{department.DepartmentId}",department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id) {
            var department = await db.Departments.FindAsync(id);
            if (department == null) {
                return NotFound();
            }

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id) {
            return db.Departments.Any(e => e.DepartmentId == id);
        }
    }
}

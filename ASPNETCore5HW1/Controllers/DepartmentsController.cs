using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;

namespace ASPNETCore5HW1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase {
        private readonly ContosoUniversityContext db;

        public DepartmentsController(ContosoUniversityContext context) => db = context;

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments() => await db.Departments.ToListAsync();

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id) {
            Department department = await db.Departments.FindAsync(id);

            if (department == null) {
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
            db.Departments.FromSqlInterpolated($"dbo.Department_Update {department.DepartmentId},{departmentVM.Name},{departmentVM.Budget},{departmentVM.StartDate},{departmentVM.InstructorId},{department.RowVersion}");

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Department> PostDepartment(Department departmentVM) {
            db.Departments.FromSqlInterpolated($"dbo.Department_Insert {departmentVM.Name},{departmentVM.Budget},{departmentVM.StartDate},{departmentVM.InstructorId}");
            return Ok();
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id) {
            var department = await GetDepartment(id);
            db.Departments.FromSqlInterpolated($"dbo.Department_Delete {department.Value.DepartmentId},{department.Value.RowVersion}");
            return Ok();
        }

        private bool DepartmentExists(int id) => db.Departments.Any(e => e.DepartmentId == id);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult PutDepartment(int id, Department departmentVM) {
            if (!DepartmentExists(id)) {
                return NotFound();
            }

            var department = db.Departments.Find(id);
            if (department != null) {
                db.Database.ExecuteSqlRaw("EXEC [dbo].[Department_Update] @DepartmentID,@Name,@Budget,@StartDate,@InstructorID,@RowVersion_Original",
                    new SqlParameter("@DepartmentID", id),
                    new SqlParameter("@Name", departmentVM.Name),
                    new SqlParameter("@Budget", departmentVM.Budget),
                    new SqlParameter("@StartDate", departmentVM.StartDate),
                    new SqlParameter("@InstructorID", departmentVM.InstructorId),
                    new SqlParameter("@RowVersion_Original", department.RowVersion)
                );
            }

            // 更新dbcontext cache
            db.Entry(department).Reload();

            return Created($"api/Departments/{id}", department);
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
            var department = db.Departments.Find(id);
            if (department != null) {
                db.Database.ExecuteSqlRaw("EXEC [dbo].[Department_Delete] @DepartmentID,@RowVersion_Original",
                    new SqlParameter("@DepartmentID", department.DepartmentId),
                    new SqlParameter("@RowVersion_Original", department.RowVersion)
                );
            }

            return NoContent();
        }

        private bool DepartmentExists(int id) => db.Departments.Any(e => e.DepartmentId == id);
    }
}

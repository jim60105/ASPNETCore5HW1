using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore5HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;
        private readonly ContosoUniversityContextProcedures procedures;

        public DepartmentsController(
            ContosoUniversityContext context,
            ContosoUniversityContextProcedures _procedures)
        {
            db = context;
            procedures = _procedures;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
            => await db.Departments.ToListAsync();

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department departmentVM)
        {
            if (!DepartmentExists(id))
            {
                return NotFound();
            }

            var department = db.Departments.Find(id);
            if (department != null)
            {
                await procedures.Department_Update(
                    department.DepartmentId,
                    departmentVM.Name,
                    departmentVM.Budget,
                    departmentVM.StartDate,
                    departmentVM.InstructorId,
                    department.RowVersion);
            }

            // 更新dbcontext cache
            db.Entry(department).Reload();

            return Ok();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartmentAsync(Department departmentVM)
        {
            await procedures.Department_Insert(
                departmentVM.Name,
                departmentVM.Budget,
                departmentVM.StartDate,
                departmentVM.InstructorId);
            return Ok();
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await db.Departments.FindAsync(id);
            if (department != null)
            {
                await procedures.Department_Delete(
                    id,
                    department.RowVersion
                );
            }

            return NoContent();
        }

        private bool DepartmentExists(int id) => db.Departments.Any(e => e.DepartmentId == id);

        [HttpGet("DepartmentCoursesCount/{id}")]
        public ActionResult<int> GetDepartmentCoursesCount(int id)
        {
            var result = db.VwDepartmentCourseCounts.FromSqlInterpolated($"SELECT * from [dbo].[vwDepartmentCourseCount] WHERE [DepartmentID] = {id}");
            return result.First().CourseCount;
        }
    }
}

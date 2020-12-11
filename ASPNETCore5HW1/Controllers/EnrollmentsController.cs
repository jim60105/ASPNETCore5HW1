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
    public class EnrollmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public EnrollmentsController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
            return await db.Enrollments.ToListAsync();
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(int id)
        {
            var enrollment = await db.Enrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/Enrollments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnrollment(int id, EnrollmentEditVM enrollmentVM) {
            if (!EnrollmentExists(id)) {
                return NotFound();
            }

            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            enrollment?.InjectFrom(enrollmentVM);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<Enrollment>> PostEnrollment(EnrollmentEditVM enrollmentVM) {
            Enrollment enrollment = new Enrollment();
            enrollment.InjectFrom(enrollmentVM);

            var entry = db.Enrollments.Add(enrollment);
            await db.SaveChangesAsync();

            enrollment =(Enrollment) entry.GetDatabaseValues().ToObject();
            return Created($"/api/Enrollments/{enrollment.EnrollmentId}",enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(int id) {
            var enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null) {
                return NotFound();
            }

            db.Enrollments.Remove(enrollment);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool EnrollmentExists(int id) {
            return db.Enrollments.Any(e => e.EnrollmentId == id);
        }
    }
}

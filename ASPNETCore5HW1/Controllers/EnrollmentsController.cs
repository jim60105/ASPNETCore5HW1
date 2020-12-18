using System.Collections.Generic;
using System.Linq;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;
using Repository;

namespace ASPNETCore5HW1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase {
        private readonly Repository<Enrollment> repo;

        public EnrollmentsController(Repository<Enrollment> context) => repo = context;
        private Enrollment FindById(int id) => repo.FindByCondition(e => e.EnrollmentId == id).FirstOrDefault();

        // GET: api/Enrollments
        [HttpGet]
        public ActionResult<IEnumerable<Enrollment>> GetEnrollments() => repo.FindAll().ToList();

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public ActionResult<Enrollment> GetEnrollment(int id) {
            Enrollment enrollment = FindById(id);

            if (enrollment == null) {
                return NotFound();
            }

            return enrollment;
        }

        // PUT: api/Enrollments/5
        [HttpPut("{id}")]
        public IActionResult PutEnrollment(int id, EnrollmentEditVM enrollmentVM) {
            Enrollment enrollment = FindById(id);
            if (null == enrollment) {
                return NotFound();
            }

            enrollment?.InjectFrom(enrollmentVM);
            repo.Update(enrollment);
            repo.SaveChanges();

            return NoContent();
        }

        // POST: api/Enrollments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Enrollment> PostEnrollment(EnrollmentEditVM enrollmentVM) {
            Enrollment enrollment = new Enrollment();
            enrollment.InjectFrom(enrollmentVM);

            repo.Create(enrollment);
            repo.SaveChanges();
            enrollment = repo.Reload(enrollment);

            return Created($"/api/Enrollments/{enrollment.EnrollmentId}", enrollment);
        }

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEnrollment(int id) {
            Enrollment enrollment = FindById(id);
            if (enrollment == null) {
                return NotFound();
            }

            repo.Delete(enrollment);
            repo.SaveChanges();

            return NoContent();
        }
    }
}

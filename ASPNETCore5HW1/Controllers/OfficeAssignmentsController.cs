using System.Collections.Generic;
using System.Linq;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;
using Repository;

namespace ASPNETCore5HW1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeAssignmentsController : ControllerBase {
        private readonly Repository<OfficeAssignment> repo;

        public OfficeAssignmentsController(Repository<OfficeAssignment> context) => repo = context;
        private OfficeAssignment FindById(int id) => repo.FindByCondition(o => o.InstructorId == id).FirstOrDefault();

        // GET: api/OfficeAssignments
        [HttpGet]
        public ActionResult<IEnumerable<OfficeAssignment>> GetOfficeAssignments() => repo.FindAll().ToList();

        // GET: api/OfficeAssignments/5
        [HttpGet("{id}")]
        public ActionResult<OfficeAssignment> GetOfficeAssignment(int id) {
            OfficeAssignment officeAssignment = FindById(id);

            if (officeAssignment == null) {
                return NotFound();
            }

            return officeAssignment;
        }

        // PUT: api/OfficeAssignments/5
        [HttpPut("{id}")]
        public IActionResult PutOfficeAssignment(int id, OfficeAssignmentEditVM officeAssignmentVM) {
            OfficeAssignment officeAssignment = FindById(id);
            if (null == officeAssignment) {
                return NotFound();
            }
            officeAssignment?.InjectFrom(officeAssignmentVM);
            repo.Update(officeAssignment);
            repo.SaveChanges();

            return NoContent();
        }

        // POST: api/OfficeAssignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<OfficeAssignment> PostOfficeAssignment(OfficeAssignmentEditVM officeAssignmentVM) {
            OfficeAssignment officeAssignment = new OfficeAssignment();
            officeAssignment.InjectFrom(officeAssignmentVM);

            repo.Create(officeAssignment);
            repo.SaveChanges();
            officeAssignment = repo.Reload(officeAssignment);

            return Created($"/api/OfficeAssignments/{officeAssignment.InstructorId}", officeAssignment);
        }

        // DELETE: api/OfficeAssignments/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOfficeAssignment(int id) {
            OfficeAssignment officeAssignment = FindById(id);
            if (officeAssignment == null) {
                return NotFound();
            }

            repo.Delete(officeAssignment);
            repo.SaveChanges();

            return NoContent();
        }
    }
}

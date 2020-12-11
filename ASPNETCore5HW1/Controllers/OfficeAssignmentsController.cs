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
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public OfficeAssignmentsController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/OfficeAssignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficeAssignment>>> GetOfficeAssignments()
        {
            return await db.OfficeAssignments.ToListAsync();
        }

        // GET: api/OfficeAssignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfficeAssignment>> GetOfficeAssignment(int id)
        {
            var officeAssignment = await db.OfficeAssignments.FindAsync(id);

            if (officeAssignment == null)
            {
                return NotFound();
            }

            return officeAssignment;
        }

        // PUT: api/OfficeAssignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfficeAssignment(int id, OfficeAssignmentEditVM officeAssignmentVM) {
            if (!OfficeAssignmentExists(id)) {
                return NotFound();
            }

            OfficeAssignment officeAssignment = await db.OfficeAssignments.FindAsync(id);
            officeAssignment?.InjectFrom(officeAssignmentVM);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/OfficeAssignments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<OfficeAssignment>> PostOfficeAssignment(OfficeAssignmentEditVM officeAssignmentVM) {
            OfficeAssignment officeAssignment = new OfficeAssignment();
            officeAssignment.InjectFrom(officeAssignmentVM);

            var entry = db.OfficeAssignments.Add(officeAssignment);
            await db.SaveChangesAsync();

            officeAssignment =(OfficeAssignment) entry.GetDatabaseValues().ToObject();
            return Created($"/api/OfficeAssignments/{officeAssignment.InstructorId}",officeAssignment);
        }

        // DELETE: api/OfficeAssignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficeAssignment(int id) {
            var officeAssignment = await db.OfficeAssignments.FindAsync(id);
            if (officeAssignment == null) {
                return NotFound();
            }

            db.OfficeAssignments.Remove(officeAssignment);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool OfficeAssignmentExists(int id) {
            return db.OfficeAssignments.Any(e => e.InstructorId== id);
        }
    }
}

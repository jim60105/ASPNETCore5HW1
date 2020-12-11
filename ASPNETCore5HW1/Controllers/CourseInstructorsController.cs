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
    public class CourseInstructorsController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public CourseInstructorsController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/CourseInstructors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseInstructor>>> GetCourseInstructors()
        {
            return await db.CourseInstructors.ToListAsync();
        }

        // GET: api/Courses/5:4
        [HttpGet("{CourseId}:{InstructorId}")]
        public async Task<ActionResult<CourseInstructor>> GetCourseInstructor(int CourseId,int InstructorId)
        {
            var course = await db.CourseInstructors.FindAsync(new object[] { CourseId, InstructorId});

            if (course == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST: api/CourseInstructors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<CourseInstructor>> PostCourseInstructor(CourseInstructorEditVM courseInstructorVM) {
            CourseInstructor courseInstructor = new CourseInstructor();
            courseInstructor.InjectFrom(courseInstructorVM);

            var entry = db.CourseInstructors.Add(courseInstructor);
            await db.SaveChangesAsync();

            courseInstructor=(CourseInstructor) entry.GetDatabaseValues().ToObject();
            return Created($"/api/CourseInstructors/{courseInstructor.CourseId}:{courseInstructor.InstructorId}",courseInstructor);
        }

        // DELETE: api/CourseInstructors/5:4
        [HttpDelete("{CourseId}:{InstructorId}")]
        public async Task<IActionResult> DeleteCourseInstructor(int CourseId,int InstructorId) {
            var courseInstructor = await db.CourseInstructors.FindAsync(new object[] { CourseId, InstructorId });
            if (courseInstructor == null) {
                return NotFound();
            }

            db.CourseInstructors.Remove(courseInstructor);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}

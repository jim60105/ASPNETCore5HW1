using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCore5HW1.Models;
using Omu.ValueInjecter;
using Repository;

namespace ASPNETCore5HW1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseInstructorsController : ControllerBase
    {
        private readonly Repository<CourseInstructor> repo;

        public CourseInstructorsController(Repository<CourseInstructor> context)
        {
            repo = context;
        }

        // GET: api/CourseInstructors
        [HttpGet]
        public ActionResult<IEnumerable<CourseInstructor>> GetCourseInstructors() {
            return repo.FindAll().ToList();
        }

        // GET: api/Courses/5:4
        [HttpGet("{CourseId}:{InstructorId}")]
        public ActionResult<CourseInstructor> GetCourseInstructor(int CourseId, int InstructorId) {
            var courseInstructor = repo.FindByCondition(ci => ci.CourseId == CourseId && ci.InstructorId == InstructorId).FirstOrDefault();

            if (courseInstructor== null) {
                return NotFound();
            }

            return courseInstructor;
        }

        // POST: api/CourseInstructors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<CourseInstructor> PostCourseInstructor(CourseInstructorEditVM courseInstructorVM) {
            CourseInstructor courseInstructor = new CourseInstructor();
            courseInstructor.InjectFrom(courseInstructorVM);

            repo.Create(courseInstructor);
            repo.SaveChanges();
            courseInstructor = repo.Reload(courseInstructor);

            return Created($"/api/CourseInstructors/{courseInstructor.CourseId}:{courseInstructor.InstructorId}", courseInstructor);
        }

        // DELETE: api/CourseInstructors/5:4
        [HttpDelete("{CourseId}:{InstructorId}")]
        public IActionResult DeleteCourseInstructor(int CourseId, int InstructorId) {
            var courseInstructor = repo.FindByCondition(ci => ci.CourseId == CourseId && ci.InstructorId == InstructorId).FirstOrDefault();
            if (courseInstructor == null) {
                return NotFound();
            }

            repo.Delete(courseInstructor);
            repo.SaveChanges();

            return NoContent();
        }
    }
}

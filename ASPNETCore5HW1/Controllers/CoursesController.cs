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
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public CoursesController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await db.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await db.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseEditVM courseVM) {
            if (!CourseExists(id)) {
                return NotFound();
            }

            Course course = await db.Courses.FindAsync(id);
            course?.InjectFrom(courseVM);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<Course>> PostCourse(CourseEditVM courseVM) {
            Course course = new Course();
            course.InjectFrom(courseVM);

            var entry = db.Courses.Add(course);
            await db.SaveChangesAsync();

            course =(Course) entry.GetDatabaseValues().ToObject();
            return Created($"/api/Courses/{course.CourseId}",course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id) {
            var course = await db.Courses.FindAsync(id);
            if (course == null) {
                return NotFound();
            }

            db.Courses.Remove(course);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id) {
            return db.Courses.Any(e => e.CourseId == id);
        }

        [HttpGet("/GetCourseStudents/{id}")]
        public ActionResult<IEnumerable<Person>> GetCourseStudents(int id)
        {
            var Students = db.VwCourseStudents.Where(e => e.CourseId == id).ToList();
            var result = new List<Person>();
            foreach (var student in Students) {
                var person = db.People.Find(student.StudentId);
                result.Add(person);
            }

            return result;
        }

        [HttpGet("/GetCourseStudentCount/{id}")]
        public ActionResult<int> GetCourseStudentCount(int id)
        {
            var count = db.VwCourseStudentCounts.Where(e => e.CourseId == id).FirstOrDefault();

            return count.StudentCount;
        }

    }
}

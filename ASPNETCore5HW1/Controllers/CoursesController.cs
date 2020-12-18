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
    public class CoursesController : ControllerBase
    {
        private readonly IsDeletedRepository<Course> repo;
        private readonly Repository<VwCourseStudent> repoVwCourseStudent;
        private readonly Repository<VwCourseStudentCount> repoVwCourseStudentsCount;
        private readonly IsDeletedRepository<Person> repoPerson;

        public CoursesController(IsDeletedRepository<Course> repo,
                                 Repository<VwCourseStudent> repoVwCourseStudent,
                                 Repository<VwCourseStudentCount> repoVwCourseStudentsCount,
                                 IsDeletedRepository<Person> repoPerson)
        {
            this.repo = repo;
            this.repoVwCourseStudent = repoVwCourseStudent;
            this.repoVwCourseStudentsCount = repoVwCourseStudentsCount;
            this.repoPerson = repoPerson;
        }

        // GET: api/Courses
        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetCourses() {
            return repo.FindAll().ToList();
        }

        private Course FindById(int id) => repo.FindByCondition(c => c.CourseId == id).FirstOrDefault();

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public ActionResult<Course> GetCourse(int id) {
            var course = FindById(id);

            if (course == null) {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public IActionResult PutCourse(int id, CourseEditVM courseVM) {
            var course = FindById(id);
            if (null == course) {
                return NotFound();
            }

            course?.InjectFrom(courseVM);
            repo.Update(course);
            repo.SaveChanges();

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Course> PostCourse(CourseEditVM courseVM) {
            Course course = new Course();
            course.InjectFrom(courseVM);

            var entry = repo.Create(course);
            repo.SaveChanges();

            course = (Course)entry.GetDatabaseValues().ToObject();
            return Created($"/api/Courses/{course.CourseId}", course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id) {
            var course = FindById(id);
            if (course == null) {
                return NotFound();
            }

            repo.Delete(course);
            repo.SaveChanges();

            return NoContent();
        }


        [HttpGet("/GetCourseStudents/{id}")]
        public ActionResult<IEnumerable<Person>> GetCourseStudents(int id) {
            var Students = repoVwCourseStudent.FindByCondition(e => e.CourseId == id).ToList();
            var result = new List<Person>();
            foreach (var student in Students) {
                var person = repoPerson.FindByCondition(e=>e.Id == student.StudentId).FirstOrDefault();
                result.Add(person);
            }

            return result;
        }

        [HttpGet("/GetCourseStudentCount/{id}")]
        public ActionResult<int> GetCourseStudentCount(int id) {
            var count = repoVwCourseStudentsCount.FindByCondition(e => e.CourseId == id).FirstOrDefault();

            return count.StudentCount;
        }
    }
}

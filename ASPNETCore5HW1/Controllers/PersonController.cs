using System.Collections.Generic;
using System.Linq;
using ASPNETCore5HW1.Models;
using Microsoft.AspNetCore.Mvc;
using Omu.ValueInjecter;
using Repository;

namespace ASPNETCore5HW1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase {
        private readonly IsDeletedRepository<Person> repo;

        public PeopleController(IsDeletedRepository<Person> context) => repo = context;
        private Person FindById(int id) => repo.FindByCondition(p => p.Id == id).FirstOrDefault();

        // GET: api/People
        [HttpGet]
        public ActionResult<IEnumerable<Person>> GetPeople() => repo.FindAll().ToList();

        // GET: api/People/5
        [HttpGet("{id}")]
        public ActionResult<Person> GetPerson(int id) {
            Person person = FindById(id);

            if (person == null) {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, PersonEditVM personVM) {
            Person person = FindById(id);
            if (null == person) {
                return NotFound();
            }

            person?.InjectFrom(personVM);
            repo.Update(person); //必要，為了寫入DateModified
            repo.SaveChanges();

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Person> PostPerson(PersonEditVM personVM) {
            Person person = new Person();
            person.InjectFrom(personVM);

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Person> entry = repo.Create(person);
            repo.SaveChanges();

            person = (Person)entry.GetDatabaseValues().ToObject();
            return Created($"/api/People/{person.Id}", person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id) {
            Person person = FindById(id);
            if (person == null) {
                return NotFound();
            }

            repo.Delete(person);
            repo.SaveChanges();

            return NoContent();
        }
    }
}

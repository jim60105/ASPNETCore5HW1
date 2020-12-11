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
    public class PeopleController : ControllerBase
    {
        private readonly ContosoUniversityContext db;

        public PeopleController(ContosoUniversityContext context)
        {
            db = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return await db.People.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await db.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, PersonEditVM personVM) {
            if (!PersonExists(id)) {
                return NotFound();
            }

            Person person = await db.People.FindAsync(id);
            person?.InjectFrom(personVM);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
       public async Task<ActionResult<Person>> PostPerson(PersonEditVM personVM) {
            Person person = new Person();
            person.InjectFrom(personVM);

            var entry = db.People.Add(person);
            await db.SaveChangesAsync();

            person =(Person) entry.GetDatabaseValues().ToObject();
            return Created($"/api/People/{person.Id}",person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id) {
            var person = await db.People.FindAsync(id);
            if (person == null) {
                return NotFound();
            }

            db.People.Remove(person);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id) {
            return db.People.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Macrix_REST_API_App.Models;
using System.Text.RegularExpressions;

namespace Macrix_REST_API_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly MacrixContext _context;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(MacrixContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
          if (_context.People == null)
          {
              return NotFound();
          }
            return await _context.People.ToListAsync();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
          if (_context.People == null)
          {
              return NotFound();
          }
            var person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            if(!IsPersonValid(person))
            {
                return BadRequest("One or more of the person entities has invalid data.");
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(List<Person> people)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'MacrixContext.People'  is null.");
            }
            foreach (Person person in people)
            {
                if (IsPersonValid(person))
                {
                    _context.People.Add(person);
                }
                else
                {
                    return BadRequest("One or more of the person entities has invalid data.");
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                foreach(Person person in people)
                {
                    if (PersonExists(person.Id))
                    {
                        return Conflict();
                    }
                }
                throw;
            }

            return CreatedAtAction("GetPeople", new {}, people);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            if (_context.People == null)
            {
                return NotFound();
            }
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(long id)
        {
            return (_context.People?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool IsPersonValid(Person person)
        {
            //multiple if statements for readibility
            if (string.IsNullOrEmpty(person.FirstName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(person.LastName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(person.StreetName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(person.Town))
            {
                return false;
            }
            //checks if number has proper length for a Polish standard
            if(person.PhoneNumber.ToString().Length != 9)
            {
                return false;
            }
            Regex postalCodeSchema = new Regex("^[0-9]{2}-[0-9]{3}$");
            if (!postalCodeSchema.IsMatch(person.PostalCode))
            {
                return false;
            }
            //SQLite does not have datetime type, therefore it's necessary to test if it's properly formated as a string 
            Regex dateSchema = new Regex("^(19|20)[0-9]{2}-(0[1-9]|1[1,2])-(0[1-9]|[12][0-9]|3[01])T(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
            if (!dateSchema.IsMatch(person.DateOfBirth))
            {
                return false;
            }
            return true;

        }
    }
}

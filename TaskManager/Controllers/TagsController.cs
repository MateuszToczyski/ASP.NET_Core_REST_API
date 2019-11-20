using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TagsController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Tag>> GetDetails(int id)
        {
            Tag tag = await _context.Tags
                .Where(tag => tag.Id == id)
                .Include(tag => tag.TagAssignments)
                .SingleOrDefaultAsync();

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetByTodo(int id)
        {
            Todo todo = await _context.Todos
                .Where(t => t.Id == id)
                .Include(t => t.TagAssignments)
                    .ThenInclude(ta => ta.Tag)
                        .ThenInclude(tag => tag.TagAssignments)
                .SingleOrDefaultAsync();

            if (todo == null)
            {
                return NotFound();
            }

            return todo.TagAssignments.Select(ta => ta.Tag).ToList();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tag>> Put(int id, [FromBody]Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Tags.FindAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(tag);
        }

        [HttpPost]
        public async Task<ActionResult<Tag>> Post([FromBody] Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetails), new { id = tag.Id }, tag);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Tag>> Delete(int id)
        {
            Tag tag = await _context.Tags.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(tag);
        }
    }
}
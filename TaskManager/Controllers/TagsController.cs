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
        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _context.Tags.ToListAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<Tag> GetDetails(int id)
        {
            return await _context.Tags
                .Where(tag => tag.Id == id)
                    .Include(tag => tag.TagAssignments)
                .SingleOrDefaultAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<IEnumerable<Tag>> GetByTodo(int id)
        {
            Todo todo = await _context.Todos
                .Where(t => t.Id == id)
                .Include(t => t.TagAssignments)
                    .ThenInclude(ta => ta.Tag)
                        .ThenInclude(tag => tag.TagAssignments)
                .SingleOrDefaultAsync();

            return todo.TagAssignments.Select(ta => ta.Tag).ToList();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tag>> Put(int id, [FromBody]Tag tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return Ok(tag);
        }
    }
}
using System;
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
    public class TodosController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TodosController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<Todo> GetDetails(int id)
        {
            return await _context.Todos
                .Where(todo => todo.Id == id)
                .Include(todo => todo.TagAssignments)
                .SingleOrDefaultAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<IEnumerable<Todo>> GetByProject(int id)
        {
            return await _context.Todos
                .Where(todo => todo.ProjectId == id)
                .Include(todo => todo.TagAssignments)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<IEnumerable<Todo>> GetByTag(int id)
        {
            Tag tag = await _context.Tags
                .Where(t => t.Id == id)
                .Include(t => t.TagAssignments)
                    .ThenInclude(ta => ta.Todo)
                        .ThenInclude(todo => todo.TagAssignments)
                .SingleOrDefaultAsync();

            return tag.TagAssignments.Select(ta => ta.Todo).ToList();
        }

        [HttpGet("{date}")]
        [Route("[action]/{date}")]
        public async Task<IEnumerable<Todo>> GetByDate(DateTime date)
        {
            return await _context.Todos
                .Where(todo => todo.Date == date)
                .Include(todo => todo.TagAssignments)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> Put(int id, [FromBody]Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Post([FromBody] Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetails), new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Todo>> Delete(int id)
        {
            Todo todo = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }
    }
}
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
    public class ProjectsController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public ProjectsController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Project>>> GetActive()
        {
            return await _context.Projects
                .Where(project => project.IsArchived == false)
                .ToListAsync();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Project>>> GetArchived()
        {
            return await _context.Projects
                .Where(project => project.IsArchived == true)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Project>> GetDetails(int id)
        {
            Project project =  await _context.Projects
                .Include(project => project.Todoes)
                    .ThenInclude(todo => todo.TagAssignments)
                .Where(project => project.Id == id)
                .SingleOrDefaultAsync();

            if (project == null)
            {
                NotFound();
            }

            return project;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> Put(int id, [FromBody]Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Projects.FindAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Post([FromBody] Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetails), new { id = project.Id }, project);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            Project project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }
    }
}

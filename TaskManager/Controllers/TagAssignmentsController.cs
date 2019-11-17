using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagAssignmentsController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TagAssignmentsController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet("{tagId}/{todoId}")]
        public async Task<TagAssignment> Get(int tagId, int todoId)
        {
            return await _context.TagAssignments
                .Where(ta => ta.TagId == tagId && ta.TodoId == todoId)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TagAssignment>> Post([FromBody] TagAssignment tagAssignment)
        {
            await _context.TagAssignments.AddAsync(tagAssignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { tagId = tagAssignment.TagId, todoId = tagAssignment.TodoId }, tagAssignment);
        }

        [HttpDelete("{tagId}/{todoId}")]
        public async Task<ActionResult<TagAssignment>> Delete(int tagId, int todoId)
        {
            TagAssignment tagAssignment = await _context.TagAssignments
                .Where(ta => ta.TagId == tagId && ta.TodoId == todoId)
                .FirstOrDefaultAsync();

            _context.TagAssignments.Remove(tagAssignment);
            await _context.SaveChangesAsync();

            return Ok(tagAssignment);
        }
    }
}
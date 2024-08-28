using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PendingTaskController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public PendingTaskController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/PendingTask
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingTask>>> GetTasks()
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/PendingTask/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PendingTask>> GetPendingTask(int id)
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            var pendingTask = await _context.Tasks.FindAsync(id);

            if (pendingTask == null)
            {
                return NotFound();
            }

            return pendingTask;
        }

        // PUT: api/PendingTask/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPendingTask(int id, PendingTask pendingTask)
        {
            if (id != pendingTask.ID)
            {
                return BadRequest();
            }

            _context.Entry(pendingTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingTaskExists(id))
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

        // POST: api/PendingTask
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PendingTask>> PostPendingTask(PendingTask pendingTask)
        {
          if (_context.Tasks == null)
          {
              return Problem("Entity set 'TaskDbContext.Tasks'  is null.");
          }
            _context.Tasks.Add(pendingTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPendingTask", new { id = pendingTask.ID }, pendingTask);
        }

        // DELETE: api/PendingTask/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePendingTask(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var pendingTask = await _context.Tasks.FindAsync(id);
            if (pendingTask == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(pendingTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PendingTaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

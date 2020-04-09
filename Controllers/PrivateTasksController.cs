using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _netCoreBackend.Handlers;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserTask = _netCoreBackend.Models.Task;


namespace _netCoreBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PrivateTasksController : Controller
    {
        private readonly ManagerContext _ctx;
        private readonly JWTSettings _jwtSettings;

        public PrivateTasksController(ManagerContext ctx , IOptions<JWTSettings> jwtSettings)
        {
            _ctx = ctx;
            _jwtSettings = jwtSettings.Value;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPrivateTasks()
        {
            string username = HttpContext.User.Identity.Name;

            var user = await _ctx.Credentials
                .AsNoTracking()
                .Include(c => c.PrivateTasks)
                .FirstOrDefaultAsync(c => c.Username == username);

            return Ok( 
                    new
                    {
                        tasks = user.PrivateTasks,
                        count = user.PrivateTasks.Count()
                    });
        }

        [HttpGet("task/{id:int}") ]
        public IActionResult GetAPrivateTask(int id)
        {
            var task = RetreiveTask(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, PrivateTask task)
        {
            if (id != task.TaskId)
            {
                return BadRequest();
            }

            var requestedTask = await _ctx.PrivateTasks
                .SingleOrDefaultAsync(t => t.TaskId == id);

            if (requestedTask == null) 
            {
                return NotFound();
            }
            
            //picking desired attributes in case id or ownership was wrongly changed
            requestedTask.Title = task.Title;
            requestedTask.Description = task.Description;
            requestedTask.Checklists = task.Checklists;
            

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ) when (!TaskExists(id))
            {
                return NotFound();
            }

            return NoContent();

        }
        

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            string username = HttpContext.User.Identity.Name;
            var task = RetreiveTask(id);

            if (task == null)
            {
                return NotFound();
            }

            var privateTasks = _ctx.PrivateTasks
                    .First(t => t.TaskId == id);

            _ctx.PrivateTasks.Remove(privateTasks);

            await _ctx.SaveChangesAsync();

            return Ok(task.Title);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivateTask(PrivateTask task)
        {
            PrivateTask newTask = new PrivateTask()
            {
                Title = task.Title,
                Deadline = task.Deadline,
                Description = task.Description,
                Checklists = task.Checklists,
                Status = task.Status 
            };

            var username = HttpContext.User.Identity.Name;

            var user = await _ctx.Credentials
                .SingleOrDefaultAsync(u => u.Username == username);
            
            _ctx.Entry(user)
                .Collection(u => u.PrivateTasks)
                .Load();
            
            if(!user.PrivateTasks.Any())
                user.PrivateTasks = new List<PrivateTask>();
               
            user.PrivateTasks.Add(newTask);

            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAPrivateTask), new {id = newTask.TaskId }, newTask);

        }

        private UserTask RetreiveTask(int taskId)
        {
            string username = HttpContext.User.Identity.Name;

            var user =  _ctx.Credentials.Single( cr => cr.Username == username );
            
            //validate that requested task belong to that user
            
            _ctx.Entry(user)
                .Collection(cr=> cr.PrivateTasks)
                .Load();

            return user.PrivateTasks.SingleOrDefault(t=>t.TaskId == taskId);
        }

        private bool TaskExists(int id) => 
            _ctx.PrivateTasks.Any(t => t.TaskId == id);

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _netCoreBackend.Models;
using _netCoreBackend.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Task = _netCoreBackend.Models.Task;

namespace _netCoreBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PrivateTasksController : Controller
    {
        private readonly ManagerContext _ctx;

        public PrivateTasksController(ManagerContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet]
        public  IActionResult GetPrivateTasks()
        {
            string email = HttpContext.User.Identity.Name;
            var tasks = _ctx.PrivateTasks
                .Include(task => task.User)
                .Where(t => t.User.Email == email)
                .ToList();
                
            var tasksDTO = new List<PrivateTaskDTO>();
            
            tasks.ForEach( task => tasksDTO.Add( new PrivateTaskDTO{ Title = task.Title, CreatedDate = task.CreatedDate} ) );

            return Ok( 
                    new
                    {
                        tasks = tasksDTO,
                        count = tasksDTO.Count()
                    }
                );
        }

        [HttpGet("task/{id:int}") ]
        public async Task<IActionResult> GetAPrivateTask(int id)
        {
            var task = await _ctx.PrivateTasks
                .SingleOrDefaultAsync( pt=> pt.SharedTaskId == id );

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromForm]PrivateTask task)
        {
            if (id != task.SharedTaskId)
            {
                return BadRequest();
            }

            var requestedTask = await _ctx.PrivateTasks.
                SingleOrDefaultAsync(t => t.SharedTaskId == id);

            if (requestedTask == null || id != requestedTask.OwnerId) 
            {
                return NotFound();
            }
            
            requestedTask.Title = task.Title;
            requestedTask.Description = task.Description;
            requestedTask.Checklists = task.Checklists;
            requestedTask.Status = task.Status;

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _ctx.PrivateTasks
                .SingleOrDefaultAsync(t => t.SharedTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivateTask([FromForm] PrivateTask task)
        {
            if (task.Status == null)
            {
                task.Status = Status.Open;
            }
            
            PrivateTask newTask = new PrivateTask()
            {
                Title = task.Title,
                CreatedDate = task.CreatedDate,
                Description = task.Description,
                Checklists = task.Checklists,
            };

            var email = HttpContext.User.Identity.Name;

            var user = await _ctx.Users
                .SingleOrDefaultAsync(u => u.Email ==email);
            
            _ctx.Entry(user)
                .Collection(u => u.PrivateTasks)
                .Load();
            
            
            
            if(!user.PrivateTasks.Any())
            {
                user.PrivateTasks = new List<PrivateTask>();
                user.PrivateTasks.Add(task);
            }

            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAPrivateTask), new {id = newTask.SharedTaskId }, newTask);

        }

        private bool TaskExists(int id) => 
            _ctx.PrivateTasks.Any(t => t.SharedTaskId == id);

    }
}
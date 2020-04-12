using System;
using System.Linq;
using System.Threading.Tasks;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = _netCoreBackend.Models.Task;

namespace _netCoreBackend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SharedTasksController:Controller
    {
        private readonly ManagerContext _ctx;

        public SharedTasksController(ManagerContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetGroupTasks(int id)
        {
            string username = HttpContext.User.Identity.Name;
            
            var member = 
                _ctx.Memberships
                    .FirstOrDefault( member => member.MemberUsername == username && member.GroupID == id );
            
            if ( member == null )
            {
                return Unauthorized();
            }

            var group = _ctx.Groups.Find(id);

            var sharedTasks = _ctx.SharedTasks
                .AsNoTracking()
                .Where(st => st.GroupId == id);

            return Ok(new
            {
                tasks = sharedTasks,
                belongsTo = group
            });
        }

        [HttpPost("group/{id:int}")]
        public IActionResult CreateTask(int id, SharedTasks task)
        {
            string username = HttpContext.User.Identity.Name;

            var group = _ctx.Groups
                .Include(g => g.SharedTasks)
                .FirstOrDefault(g => g.GroupId == id);
            
            if (group == null || group.AdminUsername != username)
            {
                return Unauthorized();
            }

            task.GroupId = id;
            _ctx.SharedTasks.Add(task);

            _ctx.SaveChanges();

            return Ok(task);
        }
        
        [HttpPut ("group/{groupId:int}/task/{taskId:int}")]
        public async Task<IActionResult> Update(int groupId, int taskId, SharedTasks task)
        {
            string username = HttpContext.User.Identity.Name;
            if (taskId != task.TaskId)
            {
                return BadRequest();
            }

            //verify group and task
            var group = _ctx.Groups
                .Include(g => g.SharedTasks)
                .ToList()
                .FirstOrDefault(g => g.GroupId == groupId);
            
            if (group == null || group.AdminUsername != username)
            {
                return Unauthorized();
            }

            int index = group.SharedTasks.FindIndex(t => t.GroupId == taskId);
            if (index == -1)
                return BadRequest();

            group.SharedTasks[index] = task;
            
            await _ctx.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            string username = HttpContext.User.Identity.Name;

            var task = _ctx.SharedTasks
                .Include(t => t.Group)
                .FirstOrDefault(t => t.TaskId == id && t.Group.AdminUsername == username);

            if (task == null)
                return NotFound();

            _ctx.SharedTasks.Remove(task);

            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}
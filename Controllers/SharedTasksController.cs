using System.Linq;
using System.Threading.Tasks;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("member/group/{id}")]
        public IActionResult GetGroupTasks(int id)
        {
            string username = HttpContext.User.Identity.Name;
            
            //check if user allowed to see those tasks: 
            var member =  _ctx.Memberships.Find(username, id);

            if (member == null)
            {
                return BadRequest();
            }

            var sharedTasks = _ctx.SharedTasks
                .Where(st => st.GroupId == id);

            return Ok(sharedTasks);
        }

        [HttpGet("admin/group/{id:int}")]
        public IActionResult GetAdminGroups(int id)
        {
            string username = HttpContext.User.Identity.Name;

            var group = _ctx.Groups.Find(id);
            
            //if group doesnt exists or exists and does not belong to user
            if (group == null || group.AdminUsername != username)
            {
                return Unauthorized();
            }

            _ctx.Entry(group)
                .Collection(g => g.SharedTasks)
                .Load();

            return Ok(group);
        }

        [HttpPost("group/{id:int}")]
        public async Task<IActionResult> CreateTask(int id, SharedTasks task)
        {
            string username = HttpContext.User.Identity.Name;

            var group = _ctx.Groups
                .Include(g => g.SharedTasks)
                .FirstOrDefault(g => g.GroupId == id);
            
            if (group == null || group.AdminUsername != username)
            {
                return Unauthorized();
            }
            
            group.SharedTasks.Add(task);

            await _ctx.SaveChangesAsync();

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

            group.SharedTasks[index].Title = task.Title;
            group.SharedTasks[index].Description = task.Description;
            group.SharedTasks[index].Checklists = task.Checklists;
            group.SharedTasks[index].AdminComments = task.AdminComments;
            
            await _ctx.SaveChangesAsync();

            return Ok(task);
        }
        
        
    }
}
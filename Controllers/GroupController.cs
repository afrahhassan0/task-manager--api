using System;
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
    public class GroupController: Controller
    {
        private readonly ManagerContext _ctx;

        public GroupController(ManagerContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult GetAdminGroups()
        {
            string username = HttpContext.User.Identity.Name;

            var userGroups = _ctx.Groups
                .AsNoTracking()
                .Where(group => group.AdminUsername == username);
            return Ok(new
            {
                groups = userGroups
            });
        }

        [HttpGet("member")]
        public IActionResult GetMemberGroups()
        {
            string username = HttpContext.User.Identity.Name;

            var userGroups = _ctx.Memberships
                .AsNoTracking()
                .Include(m=>m.Group)
                .Where(m => m.MemberUsername == username);
            
            return Ok(new
            {
                groups = userGroups
            });   
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup( GroupDTO groupInput )
        {
            string username = HttpContext.User.Identity.Name;
            
            Group newGroup = new Group()
            {
                AdminUsername = username,
                title = groupInput.Title,
                Description = groupInput.Description,
                CreatedDate = DateTime.Now.ToString("MM/dd/yyyy")
            };
            
            _ctx.Groups.Add(newGroup);
            

            try
            {
                await _ctx.SaveChangesAsync();
                //make the admin also a member of his own group
                _ctx.Memberships.Add(new UserGroup()
                {
                    MemberUsername = username,
                    GroupID = newGroup.GroupId,
                });
                await _ctx.SaveChangesAsync();
                return Ok( newGroup );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
            
        }

        [HttpPut ("{id:int}")]
        public async Task<IActionResult> EditGroup(int id, GroupDTO groupInput)
        {
            string username = HttpContext.User.Identity.Name;
            if (id != groupInput.GroupId)
            {
                return BadRequest();
            }

            var group = _ctx.Groups.Find(id);
            
            if (group.AdminUsername != username)
                return Unauthorized();

            group.title = groupInput.Title;
            group.Description = groupInput.Description;

            try
            {
                await _ctx.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception )
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            string username = HttpContext.User.Identity.Name;

            var group = _ctx.Groups.Find(id);
            
            if (group == null)
                return BadRequest();
            if (group.AdminUsername != username)
                return Unauthorized();

            _ctx.Groups.Remove(group);

            await _ctx.SaveChangesAsync();

            return NoContent();
        }


        

    }
}
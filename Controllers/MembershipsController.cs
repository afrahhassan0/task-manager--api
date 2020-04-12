using System;
using System.Collections.Generic;
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
    public class MembershipsController : Controller
    {
        private readonly ManagerContext _ctx;

        public MembershipsController(ManagerContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("group/{groupId:int}")]
        public async Task<IActionResult> RegisterMembership(int groupId, string[] memberUsernames )
        {
            string username = HttpContext.User.Identity.Name;

            if (!AdminIsValid(username, groupId))
            {
                return Unauthorized();
            }

            var memberships = _ctx.Memberships;
            
            //if any of the entries is faulty then nothing gets saved. A whole transaction
            try
            {
                foreach (var member in memberUsernames)
                {
                    memberships.Add(new UserGroup()
                    {
                        GroupID = groupId,
                        MemberUsername = member
                    });
                }

                await _ctx.SaveChangesAsync();
                
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }

        [HttpGet("group")]
        public IActionResult GetOrgMembers()
        {
            string username = HttpContext.User.Identity.Name;
            
            var org = _ctx.Credentials.Find(username).OrganizationName;

            var colleagues =  _ctx.Credentials.AsNoTracking()
                .Include( c => c.User )
                .Where(c => c.OrganizationName == org && c.Username != username)
                .ToList();

            List<UserDTO> colleaguesBrief = new List<UserDTO>();
            
            colleagues.ForEach(colleague =>
            {
                colleaguesBrief.Add( new UserDTO()
                {
                    FullName = colleague.User.FullName,
                    Username = colleague.Username,
                    Email = colleague.User.Email
                });
            });

            return Ok(colleaguesBrief);
        }

        [HttpGet("group/{groupId:int}")]
        public IActionResult GetMembers(int groupId)
        {
            string username = HttpContext.User.Identity.Name;

            var members = _ctx.Memberships
                .Where(m => m.GroupID == groupId );

            return Ok(members);
        }
            

        [HttpPost("group/addMember/{groupId:int}")]
        public async Task<IActionResult> RegisterOneMember(int groupId, string memberUsername)
        {
            string username = HttpContext.User.Identity.Name;

            if (!AdminIsValid(username, groupId))
                return Unauthorized();

            var memberships = _ctx.Memberships;

            try
            {
                memberships.Add(new UserGroup()
                {
                    GroupID = groupId,
                    MemberUsername = memberUsername
                });

                await _ctx.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

        }


        [HttpDelete("group/{groupId:int}/member/{memberUsername:alpha}")]
        public IActionResult DeleteMember(int groupId, string memberUsername)
        {
            string username = HttpContext.User.Identity.Name;

            if (!AdminIsValid(username, groupId))
                return Unauthorized();

            var member = _ctx.Memberships.Find(memberUsername, groupId);

            if (member == null)
                return NotFound();

            _ctx.Memberships.Remove(member);
            _ctx.SaveChanges();

            return NoContent();
        }
        
        private bool AdminIsValid(string username, int groupId)
        {
            var group = _ctx.Groups.Find(groupId);
            return  group == null || group.AdminUsername == username;
        }
    }
    
    
}
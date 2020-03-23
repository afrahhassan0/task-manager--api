using System.Collections.Generic;
using System.Threading.Tasks;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet]
        //public async Task<ActionResult<List<Group>>> GetGroups()
        
        

    }
}
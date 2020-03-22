using System;
using System.Threading.Tasks;
using System.Linq;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;


namespace _netCoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ManagerContext _ctx;

        public UsersController(ManagerContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _ctx.Users
                .Include(u => u.Credentials)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        
        
        
        [HttpPost("UserSignUp")]
        public  ActionResult<UserDTO> UserSignUp([FromForm]UserDTO user)
        {
            return user; 
            //check if username exists
            /*
            if (_ctx.Credentials.Find(us.Username) != null )
            {
                return new BadRequestObjectResult("Username already exists");
            }
            
            //check if email formatted right
            if (!ValidMail(user.Email))
            {
                return BadRequest();
            }
            
            //check if mail exists
            if (_ctx.Users.Find(user.Email) != null)
            {
                return new BadRequestObjectResult("Email already exists");
            }

            var newUser = new User
                {
                    Email = user.Email,
                    Role = user.Role,
                    FullName = user.FullName,
                    About = user.About,
                    Credentials = new List<Credentials>()
                };
                newUser.Credentials.Add(user.Credentials);

                _ctx.Users.Add(newUser);
                await _ctx.SaveChangesAsync();

                return CreatedAtAction( nameof(UserSignUp), new { id=newUser.UserId, user });
                */
        }

        private bool ValidMail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost("testing")]
        public async Task<ActionResult<User>> Testing([FromForm]UserDTO user)
        {
            /*
            //check if username exists
            if (_ctx.Credentials.ToList().FirstOrDefault(c=> c.Username == user.Credentials.u) != null)
            {
                return new BadRequestObjectResult("Username already exists");
            }*/
            
            //check if email formatted right
            if (!ValidMail(user.Email))
            {
                return BadRequest();
            }
            
            //check if mail exists
            if (_ctx.Users.FirstOrDefault(u=>u.Email == user.Email) != null)
            {
                return new BadRequestObjectResult("Email already has an account");
            }

            var newUser = new User
            {
                Email = user.Email,
                Role = user.Role,
                FullName = user.FullName,
                About = user.About
            };

            _ctx.Users.Add(newUser);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction( nameof(UserSignUp), new { id=newUser.UserId, user });

        }

        [HttpPost("TestCred/{id}", Name = "TestCred")]
        public async Task<ActionResult<Credentials>> TestCred([FromForm] Credentials credentials, int id)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            
            //check username
            if (_ctx.Credentials.FirstOrDefault(c => c.Username == credentials.Username) != null)
            {
                return new BadRequestObjectResult("Username exists.");
            }

            if (!user.Credentials.Any())
            {
                user.Credentials = new List<Credentials>();
            }

            user.Credentials.Add(credentials);

            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(UserSignUp), new {updatedUser = user});

        }
    }
}
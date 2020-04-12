using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using _netCoreBackend.Handlers;
using _netCoreBackend.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Task = _netCoreBackend.Models.Task;


namespace _netCoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ManagerContext _ctx;
        private readonly JWTSettings _jwtSettings;

        public UsersController(ManagerContext ctx,
            IOptions<JWTSettings> jwtSettings
        )
        {
            _ctx = ctx;
            _jwtSettings = jwtSettings.Value;
        }

        
        [Authorize]
        [HttpGet("authorization/{groupId:int}")]
        public IActionResult GetAuth(int groupId)
        {
            string username = HttpContext.User.Identity.Name;

            var group = _ctx.Groups.Find(groupId);
            if (group == null)
                return NotFound();

            string roleInGroup = (group.AdminUsername == username) ? "ADMIN" : "MEMBER";

            return Ok(new
            {
                role = roleInGroup
            });
        }
        
        
        [HttpGet("{id}") , ActionName(nameof(GetUser))]
        public ActionResult<User> GetUser(int id)
        {
            var user = _ctx.Users
                .Include(u => u.Credentials)
                .FirstOrDefault( u=> u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        
        
        [HttpPost("signup")]
        public  IActionResult SignUp(UserDTO user)
        {
            
            //check if username exists
            if (_ctx.Credentials.Find(user.Username) != null )
            {
                return new BadRequestObjectResult("Username already exists");
            }
            
            //check if email formatted right
            if (!ValidMail(user.Email))
            {
                return new BadRequestObjectResult("That's not an email");
            }
            
            //check if mail exists
            if (_ctx.Users.SingleOrDefault(u=>u.Email == user.Email) != null)
            {
                return new BadRequestObjectResult("Email already exists");
            }

            var newUser = CreateUser(user);

            _ctx.Users.Add(newUser);
            _ctx.SaveChanges();
            
            
            var jwtToken = GenerateAccessToken(user.Email, user.Username);
            Response.Headers.Add("Authentication" , jwtToken);
            user.Password = "";
            
            return Ok(user);
                
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO user)
        {
            if (!ValidMail(user.Email))
            {
                return new BadRequestObjectResult("That's not an email");
            }
            
            var requested = await _ctx.Users
                .AsNoTracking()
                .Include(u=> u.Credentials)
                .SingleOrDefaultAsync(u => u.Email == user.Email);
            
            
            if (requested == null ) 
            {
                return new NotFoundObjectResult("There's no user with that email. Sign up?");
            }

            string username = user.Username.Replace(" ", string.Empty);
            
            var creds = requested.Credentials
                .FirstOrDefault(cr => user.Username == cr.Username && cr.Password == user.Password);
            
            if ( !requested.Credentials.Any() || creds == null )
            {
                return new BadRequestObjectResult("Wrong Credentials, try again please");
            }
            
            var jwtToken = GenerateAccessToken(user.Email, user.Username);
            
            Response.Headers.Add("Authentication" , jwtToken);

            creds.Password = "";

            return Ok( new 
            {
                credentials = creds,
                role = requested.Role
            });
        }

        private string GenerateAccessToken(string email, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes((_jwtSettings.SecretKey));
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwtToken);
        }

        private User CreateUser(UserDTO user)
        {

            int role = (int)user.Role;

            switch (role)
            {
                case 0:
                    return new Admin()
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        About = user.About,
                        JobTitle = user.JobTitle,
                        PhoneNumber = user.PhoneNumber,
                        Role = Role.Admin,
                        Credentials = new List<Credentials>()
                        {
                            new Credentials()
                            {
                                Username = user.Username,
                                Password = user.Password,
                                OrganizationName = user.OrganizationName
                            } 
                        }
                    };

                case 1:
                    return new User()
                    {
                        Email = user.Email,
                        FullName = user.FullName,
                        About = user.About,
                        Role = Role.Member,
                        Credentials = new List<Credentials>()
                        {
                            new Credentials() 
                            { 
                                Username = user.Username,
                                Password = user.Password,
                                OrganizationName = user.OrganizationName 
                            }
                        }
                    };
                default: return null;
            }
        }

        private bool ValidMail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}
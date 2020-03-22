using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using _netCoreBackend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace _netCoreBackend.Handlers
{
    public class BasicAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ManagerContext _ctx;
        
        public BasicAuthentication(
            IOptionsMonitor<AuthenticationSchemeOptions> optionsMonitor,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ManagerContext ctx
        ) : base(optionsMonitor, logger, encoder, clock)
        {
            _ctx = ctx;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //check is header exists:
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No header");
            }

            try
            {
                var header = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var headerInBytes = Convert.FromBase64String(header.Parameter);
                string[] credentials = Encoding.UTF8.GetString(headerInBytes).Split(":");

                string username = credentials[0];
                string email = credentials[1];
                string password = credentials[2];

                //validate from db:
                var user = await _ctx.Credentials
                    .FindAsync(username);
                
                _ctx.Entry(user)
                    .Reference(u => u.User)
                    .Load();
                
                if (!user.User.Email.Equals(email) || !user.Password.Equals(password))
                {
                    return AuthenticateResult.Fail("Wrong credentials");
                }
                
                //else create an identity:
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.User.Email)
                };
                
                var identity = new ClaimsIdentity(claims , Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal , Scheme.Name);

                return AuthenticateResult.Success(ticket);

            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Error");
            }
        }
    }
}
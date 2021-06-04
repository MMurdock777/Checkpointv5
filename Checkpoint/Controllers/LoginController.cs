using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Checkpoint.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Grpc.Net.Client;
using Sentry;

namespace Checkpoint.Controllers
{

    [Route("auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IHub _sentryHub;

        public LoginController(IHub sentryHub) => _sentryHub = sentryHub;

        private async Task Authenticate(string userName)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            childSpan?.Finish(SpanStatus.Ok);
        }

        [HttpPost, HttpOptions]
        [Route("~/login")]
        public async Task<IActionResult> Login([FromBody]LoginData data)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            if (ModelState.IsValid)
            {
  
                var channel = GrpcChannel.ForAddress("http://localhost:5002");
                var client = new UserInfo.UserInfoClient(channel);
                try
                {
                    var reply = await client.LoginAsync(new LoginRequest
                    {
                        Login = data.Login,
                        Password = data.Password
                    });
                    if (reply.Success)
                    {
                        await Authenticate(data.Login); 
                        return Ok();
                    }
                    childSpan?.Finish(SpanStatus.Ok);
                    return Ok("Authentication failed");
                    
                }
                catch (Exception e)
                {
                    childSpan?.Finish(SpanStatus.InternalError);
                    SentrySdk.CaptureMessage(e.Message);
                    return Ok(e.Message);
                }
            }
            return Ok();
        }

        [HttpPost, HttpOptions]
        [Route("~/logout")]
        public async Task<IActionResult> Logout()
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            childSpan?.Finish(SpanStatus.Ok);
            return Ok();
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KekpointCool.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Grpc.Net.Client;

namespace KekpointCool.Controllers
{
    [Route("auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost, HttpOptions]
        [Route("~/login")]
        public async Task<IActionResult> Login([FromBody]LoginData data)
        {
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
                    return Ok("Authentication failed");

                }
                catch (Exception e)
                {
                    return Ok(e.Message);
                }
            }
            return Ok();
        }

        [HttpPost, HttpOptions]
        [Route("~/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }


    }
}
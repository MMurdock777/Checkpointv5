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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]LoginData data)
        {
            if (ModelState.IsValid)
            {
                string username = null;
                //using (var connection = new SqlConnection("Server = 192.168.1.83; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; "))
                //{
                //    var command = new SqlCommand($"SELECT * FROM dbo.UserInfo WHERE ID = '{request.Userid}'", connection);
                //    command.Connection.Open();
                //    using (SqlDataReader reader = command.ExecuteReader())
                //    {
                //        if (reader.HasRows)
                //        {
                //            if (reader.Read())
                //            {
                //            }
                //        }
                //    }
                //}
                if (username != null)
                {
                    await Authenticate(username);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return Ok();
        }

        [HttpPost, HttpOptions]
        [Route("~/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        [HttpPost, HttpOptions]
        [Route("~/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] LoginData Data)
        {
            return Ok();
        }

    }
}
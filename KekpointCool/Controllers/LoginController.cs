using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KekpointCool.Models;

namespace KekpointCool.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        [Route("~/auth")]
        public async Task<IActionResult> Auth()
        {
            return Ok();
        }

        [HttpPost, HttpOptions]
        [Route("~/register")]
        public async Task<IActionResult> Register([FromBody] LoginData Data)
        {
            return Ok();
        }

    }
}
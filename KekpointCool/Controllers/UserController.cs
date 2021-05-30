using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KekpointCool.Models;

namespace KekpointCool.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("~/getusers")]
        public async Task<IActionResult> GetUsers()
        {

            return Ok();
        }

        [HttpGet, HttpOptions]
        [Route("~/setuser")]
        public async Task<IActionResult> SetUser([FromBody] User user)
        {
            return Ok();
        }
    }
}
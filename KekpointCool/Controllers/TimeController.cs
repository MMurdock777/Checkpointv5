using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KekpointCool.Controllers
{
    [Route("time")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("~/timein/{ID}")]
        public async Task<IActionResult> Timein(Guid ID)
        {
            return Ok();
        }

        [HttpGet, HttpOptions]
        [Route("~/timeout/{ID}")]
        public async Task<IActionResult> Timeout(Guid ID)
        {
            return Ok();
        }
    }
}
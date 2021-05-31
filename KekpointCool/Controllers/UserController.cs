using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KekpointCool.Models;
using Grpc.Net.Client;


namespace KekpointCool.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("~/getusers/{ID}")]
        public async Task<IActionResult> GetUsers(Guid ID)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UserInfo.UserInfoClient(channel);
            try
            {
                var reply = await client.GetUserAsync(new GetUserRequest
                {
                    
                    Userid = ID.ToString()
                });
                return Ok(reply.FirstName);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [HttpGet, HttpOptions]
        [Route("~/setuser")]
        public async Task<IActionResult> SetUser([FromBody] User user)
        {
            return Ok();
        }
    }
}
using Grpc.Net.Client;
using KekpointCool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;


namespace KekpointCool.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("~/getusers/{ID}")]
        [Authorize]
        public async Task<IActionResult> GetUsers(Guid ID)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5003");
            var client = new UserInfo.UserInfoClient(channel);
            try
            {
                var reply = await client.GetUserAsync(new GetUserRequest
                {
                    Userid = ID.ToString()
                });
                return Ok(new User()
                {
                    ID = ID,
                    Firstname = reply.FirstName,
                    Middlename = reply.MiddleName,
                    LastName = reply.LastName,
                    AccessLevel = reply.AccessLevel,
                    DateOfBirth = reply.DateOfBirth.ToDateTime().Date
                });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [HttpPost, HttpOptions]
        [Route("~/setuser")]
        [Authorize]
        public async Task<IActionResult> SetUser([FromBody] User user)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5003");
            var client = new UserInfo.UserInfoClient(channel);
            try
            {
                var reply = await client.SetUserAsync(new SetUserRequest
                {
                    Userid = user.ID == null ?  Guid.NewGuid().ToString() : user.ID.ToString(),
                    FirstName = user.Firstname,
                    MiddleName = user.Middlename,
                    LastName = user.LastName,
                    AccessLevel = user.AccessLevel,
                    DateOfBirth = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc))
                }) ;
                return Ok();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [HttpPost, HttpOptions]
        [Route("~/{ID}/setphoto")]
        [Authorize]
        public async Task<IActionResult> SetPhoto(Guid ID, [FromBody] byte[] Photo)
        {
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new UserInfo.UserInfoClient(channel);
            //try
            //{
            //    var reply = await client.GetUserAsync(new GetUserRequest
            //    {

            //        Userid = ID.ToString()
            //    });
            //    return Ok(reply.FirstName);
            //}
            //catch (Exception e)
            //{
            //    return Ok(e.Message);
            //}
            return Ok();
        }
    }
}
using Grpc.Net.Client;
using Checkpoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Sentry;



namespace Checkpoint.Controllers
{

    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHub _sentryHub;

        public UserController(IHub sentryHub) => _sentryHub = sentryHub;

        [HttpGet, HttpOptions]
        [Route("~/getusers/{ID}")]
        [Authorize]
        public async Task<IActionResult> GetUsers(Guid ID)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var channel = GrpcChannel.ForAddress("http://localhost:5002");
            var client = new UserInfo.UserInfoClient(channel);
            try
            {
                var reply = await client.GetUserAsync(new GetUserRequest
                {
                    Userid = ID.ToString()
                });
                if (reply.FirstName != ""){
                    childSpan?.Finish(SpanStatus.Ok);
                    return Ok(new Models.User()
                    {
                        ID = ID,
                        Firstname = reply.FirstName,
                        Middlename = reply.MiddleName,
                        LastName = reply.LastName,
                        AccessLevel = reply.AccessLevel,
                        DateOfBirth = reply.DateOfBirth.ToDateTime().Date
                    });
                    
                }
                childSpan?.Finish(SpanStatus.InternalError);
                return Ok();
            }
            catch (Exception e)
            {
                childSpan?.Finish(SpanStatus.InternalError);
                SentrySdk.CaptureMessage(e.Message);
                return Ok(e.Message);
            }

        }

        [HttpPut, HttpOptions]
        [Route("~/setuser")]
        [Authorize]
        public async Task<IActionResult> SetUser([FromBody] Models.User user)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var channel = GrpcChannel.ForAddress("http://localhost:5002");
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
                childSpan?.Finish(SpanStatus.Ok);
                return Ok();
            }
            catch (Exception e)
            {
                childSpan?.Finish(SpanStatus.InternalError);
                SentrySdk.CaptureMessage(e.Message);
                return Ok(e.Message);
            }
        }

        //[HttpPost, HttpOptions]
        //[Route("~/{ID}/setphoto")]
        //[Authorize]
        //public async Task<IActionResult> SetPhoto(Guid ID, [FromBody] byte[] Photo)
        //{
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
            //return Ok();
        //}
    }
}
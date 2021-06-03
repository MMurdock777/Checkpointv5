using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;

namespace KekpointCool.Controllers
{
    [Route("time")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("~/timein/{ID}")]
        [Authorize]
        public async Task<IActionResult> Timein(Guid ID)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Time.TimeClient(channel);
            try
            {
                var reply = await client.TimeInAsync(new TimeRequest { Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)),
                    Userid = ID.ToString() });

                return Ok(reply.Message);
            }
            catch(Exception e)
            {
                return Ok(e.Message);
            }
           
            
        }

        [HttpGet, HttpOptions]
        [Route("~/timeout/{ID}")]
        [Authorize]
        public async Task<IActionResult> Timeout(Guid ID)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5000");
            var client = new Time.TimeClient(channel);
            try
            {
                var reply = await client.TimeOutAsync(new TimeRequest
                {
                    Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)),
                    Userid = ID.ToString()
                });

                return Ok(reply.Message);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
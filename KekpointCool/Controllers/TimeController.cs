using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Sentry;

namespace Checkpoint.Controllers
{
    [Route("time")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly IHub _sentryHub;

        public TimeController(IHub sentryHub) => _sentryHub = sentryHub;

        [HttpGet, HttpOptions]
        [Route("~/timein/{ID}")]
        [Authorize]
        public async Task<IActionResult> Timein(Guid ID)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Time.TimeClient(channel);
            try
            {
                var reply = await client.TimeInAsync(new TimeRequest { Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)),
                    Userid = ID.ToString() });
                childSpan?.Finish(SpanStatus.Ok);
                return Ok(reply.Message);
            }
            catch(Exception e)
            {
                childSpan?.Finish(SpanStatus.InternalError);
                SentrySdk.CaptureMessage(e.Message);
                return Ok(e.Message);
            }
           
            
        }

        [HttpGet, HttpOptions]
        [Route("~/timeout/{ID}")]
        [Authorize]
        public async Task<IActionResult> Timeout(Guid ID)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Time.TimeClient(channel);
            try
            {
                var reply = await client.TimeOutAsync(new TimeRequest
                {
                    Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)),
                    Userid = ID.ToString()
                });
                childSpan?.Finish(SpanStatus.Ok);
                return Ok(reply.Message);
            }
            catch (Exception e)
            {
                childSpan?.Finish(SpanStatus.InternalError);
                SentrySdk.CaptureMessage(e.Message);
                return Ok(e.Message);
            }
        }
        [HttpGet, HttpOptions]
        [Route("~/gettime/{ID}")]
        [Authorize]
        public async Task<IActionResult> GetTime(Guid ID)
        {
            var childSpan = _sentryHub.GetSpan()?.StartChild("additional-work");
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Time.TimeClient(channel);
            try
            {
                var reply = await client.GetTimeAsync(new GetTimeRequest
                {
                    Userid = ID.ToString()
                });
                childSpan?.Finish(SpanStatus.Ok);
                return Ok(reply);
            }
            catch (Exception e)
            {
                childSpan?.Finish(SpanStatus.InternalError);
                SentrySdk.CaptureMessage(e.Message);
                return Ok(e.Message);
            }
        }
    }
}
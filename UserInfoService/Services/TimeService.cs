using System;
using Grpc.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace UserInfoService.Services
{
    public class TimeService : Time.TimeBase
    {
        private readonly ILogger<TimeService> _logger;
        public TimeService(ILogger<TimeService> logger)
        {
            _logger = logger;
        }

        public override Task<TimeOkReply> TimeIn(TimeRequest request, ServerCallContext context)
        {
            // to do
            return Task.FromResult(new TimeOkReply
            {
                Message = "All good",
            });
        }

        public override Task<TimeOkReply> TimeOut(TimeRequest request, ServerCallContext context)
        {
            // to do
            return Task.FromResult(new TimeOkReply
            {
                Message = "All good",
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace UserInfoService.Services
{
    public class UserService : UserInfo.UserInfoBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<GetUserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            // to do request.id
            return Task.FromResult(new GetUserReply
            {
                FirstName = "All good",
            });
        }

        public override Task<SetUserReply> SetUser(SetUserRequest request, ServerCallContext context)
        {
            // to do
            return Task.FromResult(new SetUserReply
            {
                Message = "All good",
            });
        }
    }
}

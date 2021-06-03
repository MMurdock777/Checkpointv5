using System;
using Grpc.Core;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient; 

namespace TimeControlService.Services
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
            using (var connection = new SqlConnection("Server = 95.165.129.223; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; "))
            {
                string format = "yyyy.MM.dd HH:mm:ss";
                var command = new SqlCommand($"INSERT INTO dbo.TimeIn (ID, TimeIn) VALUES ('{request.Userid}', '{request.Timestamp.ToDateTime().ToString(format)}') ", connection);
                command.Connection.Open();
                int number = command.ExecuteNonQuery();
               
                return Task.FromResult(new TimeOkReply
                {
                    Message = $"Добавлено объектов: {number}"
                });
            }
        }
        public override Task<TimeOkReply> TimeOut(TimeRequest request, ServerCallContext context)
        {
            using (var connection = new SqlConnection("Server = 95.165.129.223; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; "))
            {
                string format = "yyyy.MM.dd HH:mm:ss";
                var command = new SqlCommand($"INSERT INTO dbo.TimeOut (ID, TimeOut) VALUES ('{request.Userid}', '{request.Timestamp.ToDateTime().ToString(format)}') ", connection);
                command.Connection.Open();
                int number = command.ExecuteNonQuery();

                return Task.FromResult(new TimeOkReply
                {
                    Message = $"Добавлено объектов: {number}"
                });
            }

        }
    }
}

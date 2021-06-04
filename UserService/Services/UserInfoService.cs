using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;


namespace UserService.Services
{
    public class UserInfoService : UserInfo.UserInfoBase
    {
        private readonly ILogger<UserInfoService> _logger;
        public UserInfoService(ILogger<UserInfoService> logger)
        {
            _logger = logger;
        }

        public override Task<GetUserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            using (var connection = new SqlConnection("Server = 95.165.129.223; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; ")) 
            {
                var command = new SqlCommand($"SELECT * FROM dbo.UserInfo WHERE ID = '{request.Userid}'", connection); 
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) 
                    {
                        if (reader.Read())
                        {

                            Guid ID = (Guid) reader.GetValue(0);
                            string FirstName = (string) reader.GetValue(1);
                            string MiddleName = (string)reader.GetValue(2);
                            string LastName = (string) reader.GetValue(3);
                            string AccessLevel = (string)reader.GetValue(4);
                            DateTime DateOfBirth  = (DateTime)reader.GetValue(5);

                            return Task.FromResult(new GetUserReply
                            {
                                FirstName = FirstName,
                                MiddleName = MiddleName,
                                LastName = LastName,
                                AccessLevel = AccessLevel,
                                DateOfBirth = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(DateOfBirth, DateTimeKind.Utc))
                            });
                        }
                    }
                }
            };
            return Task.FromResult(new GetUserReply
            {

            });
        }

        public override Task<SetUserReply> SetUser(SetUserRequest request, ServerCallContext context)
        {
            using (var connection = new SqlConnection("Server = 95.165.129.223; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; "))
            {
                var command = new SqlCommand($"SELECT ID FROM dbo.UserInfo WHERE ID = '{request.Userid}'", connection);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    
                    if (reader.HasRows)
                    {
                        string format = "yyyy.MM.dd HH:mm:ss";
                        command = new SqlCommand($"UPDATE dbo.UserInfo SET FirstName = '{request.FirstName}', MiddleName = '{request.MiddleName}', LastName = '{request.LastName}'," +
                            $"AccessLevel = '{request.AccessLevel}', DateOfBirth = '{request.DateOfBirth.ToDateTime().Date.ToString(format)}' WHERE ID = '{request.Userid}' ", connection);
                        //command.Connection.Open();
                        int number = command.ExecuteNonQuery();
                        command.Connection.Close();
                        return Task.FromResult(new SetUserReply
                        {
                            Message = $"Обновлено объектов: {number}"
                        });
                    }
                    else
                    {
                        string format = "yyyy.MM.dd HH:mm:ss";
                        command = new SqlCommand($"INSERT INTO dbo.UserInfo (ID, FirstName, MiddleName, LastName, AccessLevel, DateOfBirth) VALUES ('{request.Userid}', '{request.FirstName}', '{request.MiddleName}', " +
                            $" '{request.LastName}', '{request.AccessLevel}', '{request.DateOfBirth.ToDateTime().Date.ToString(format)}') ", connection);
                        //command.Connection.Open();
                        int number = command.ExecuteNonQuery();
                        command.Connection.Close();

                        return Task.FromResult(new SetUserReply
                        {
                            Message = $"Добавлено объектов: {number}"
                        });
                    }
                }

            };
        }

        public override Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
        {
            using (var connection = new SqlConnection("Server = 95.165.129.223; Database = CheckpointDB; User ID = server; Password = 1580; Trusted_Connection = False; Encrypt = True; Connection Timeout = 2400; MultipleActiveResultSets = True; trustServerCertificate = True; "))
            {
                var command = new SqlCommand($"SELECT * FROM dbo.UserInfo WHERE Login = '{request.Login}' AND Password = '{request.Password}' ", connection);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        return Task.FromResult(new LoginReply
                        {
                            Success = true
                        }) ;
                        
                    }
                }
            };
            return Task.FromResult(new LoginReply
            {
                Success = false
            }) ;
        }
    }
}

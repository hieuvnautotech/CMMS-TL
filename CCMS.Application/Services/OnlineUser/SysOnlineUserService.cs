using CCMS.Application.Dtos.notice;
using CCMS.Application.Hubs;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services.OnlineUser
{
    public interface ISysOnlineUserService
    {
       // Task<dynamic> List(PageInputBase input);

        Task ForceExit(int userid, string ip);

       Task PushNotice(NotifyMessage message, List<long> userIds);

        Task<List<long>> GetAllOnlineUser();

        Task SendMessage(MessageInfo message, int userid, string ip = null);
         Task SendMessageAll(MessageInfo message);
        Task<List<OnlineUser>> GetOnlineUser(int userid);

        Task LockAccount(int userid, string ip);
        Task<List<OnlineUser>> GetList();
    }

    public class SysOnlineUserService : ISysOnlineUserService, ITransient
    {
        private readonly ISysCacheService _sysCacheService;
       private readonly IDapperRepository<OnlineUser> _sysOnlineUser; 
        private readonly IHubContext<UserHub, IUserClient> _userHubContext;

        public SysOnlineUserService(ISysCacheService sysCacheService
           , IDapperRepository<OnlineUser> sysOnlineUser
           , IHubContext<UserHub, IUserClient> chatHubContext)
        {
            _sysCacheService = sysCacheService;
            _sysOnlineUser = sysOnlineUser;
            _userHubContext = chatHubContext;
        }

        //[HttpGet("/sysOnlineUser/list")]
        //public async Task<dynamic> Get()
        //{
        //    var list = await _sysOnlineUser.AsQueryable().ToPagedListAsync(input.PageNo, input.PageSize);
        //    return list.XnPagedResult();
        //}

        // [HttpPost("/sysOnlineUser/forceExist")]
        // [NonValidation]
        public async Task ForceExit(int userid, string ip)
        {
            var lstConnections = await GetConnectionId(userid, ip);
            if (lstConnections.Any())
            {
                foreach (var connectionId in lstConnections)
                {
                   
                    await _userHubContext.Clients.Client(connectionId).ForceExist("7777777");
                    await _userHubContext.Clients.Client(connectionId).AppendNotice(new NotifyMessage
                    {

                        type = Enum.MessageType.force_exit
                    });
                   
                }
            }

            
        }

        public async Task  LockAccount(int userid, string ip)
        {
            var query = @"select * from sys_online_user where userid=@userid";
            if (!string.IsNullOrWhiteSpace(ip))
            {
                query += "  and LastLoginIp=@ip";
            }
                var list = await _sysOnlineUser.QueryAsync<OnlineUser>(query, new {userid,ip});
            var listuser = list.ToList();
            if (listuser !=null && listuser.Count==0)
            {
                //nếu user không đang online
                await _sysOnlineUser.Context.ExecuteAsync(@"
                                                        INSERT INTO [dbo].[sys_online_user]
                                                                   ([ConnectionId]
                                                                   ,[UserId]
                                                                   ,[LastTime]
                                                                   ,[Account]
                                                                   ,[Name]
                                                                   ,[LastLoginIp]
                                                                   ,[IsAccount_Locked_By_Ip]
                                                                   ,[Is_Account_Locked])
                                                             VALUES
                                                                   (@ConnectionId
                                                                   ,@UserId
                                                                   ,getdate()
                                                                   ,null
                                                                   ,null
                                                                   ,@Ip
                                                                   ,@IsAccount_Locked_By_Ip
                                                                    ,@Is_Account_Locked)
                                                        ", new { ConnectionId=  Guid.NewGuid().ToString(), 
                    UserId=userid , 
                    Ip=ip, IsAccount_Locked_By_Ip =!string.IsNullOrWhiteSpace(ip),
                    Is_Account_Locked = string.IsNullOrWhiteSpace(ip)
                });
            } else
            {
                //nếu user đang online
                query = @"update  sys_online_user
";
                if (!string.IsNullOrWhiteSpace(ip))
                {
                    query += " set  IsAccount_Locked_By_Ip=1";

                }
                else
                {
                    query += " set  Is_Account_Locked=1";
                }
                query += "  where [UserId]=@userid";
                if (!string.IsNullOrWhiteSpace(ip))
                {

                    query += "  and LastLoginIp=@ip";
                }


                await _sysOnlineUser.Context.ExecuteAsync(query, new { userid, ip });


                var lstConnections = await GetConnectionId(userid, ip);
                if (lstConnections.Any())
                {
                    foreach (var connectionId in lstConnections)
                    {
                        await _userHubContext.Clients.Client(connectionId).AppendNotice(new NotifyMessage
                        {

                            type = Enum.MessageType.lock_account
                             

                    });
                    }
                }

            }


        }
        public async Task<List<OnlineUser>> GetList()
        {
            var list = await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user");
            return list.ToList();
        }

        public async Task<List<long>> GetAllOnlineUser()
        {
            var list = await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user");
            return list.ToList().Select(a => a.UserId).ToList();
        }
        public async Task<List<OnlineUser>> GetOnlineUser(int userid)
        {
            var list = await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user where userid=@userid", new {userid});
            return list.ToList();
        }

        private async Task<List<string>> GetConnectionId(int userid,string ip)
        {
            var query = @"select * from sys_online_user where [UserId]=@userid";
            if (!string.IsNullOrWhiteSpace(ip))
            {
                query += " and LastLoginIp=@ip";
            }
            var list = await _sysOnlineUser.QueryAsync<OnlineUser>(query, new { userid,ip });
            return list.ToList().Select(a => a.ConnectionId).ToList();
        }
        public async Task SendMessage(MessageInfo message, int userid, string ip=null)
        {
            var lstConnections= await GetConnectionId(userid,ip);
            if (lstConnections.Any())
            {
                foreach (var connectionId in lstConnections)
                {
                    await _userHubContext.Clients.Client(connectionId).AppendNotice(new NotifyMessage
                    {
                         data = message,
                         type= Enum.MessageType.alert_message
                    });
                }
            }
        }

        public async Task SendMessageAll(MessageInfo message)
        {
            var list = await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user");
            var userList = list.ToList();
            if (userList.Any())
            {
                foreach (var item in userList)
                {
                    await _userHubContext.Clients.Client(item.ConnectionId).AppendNotice(new NotifyMessage
                    {
                        data = message,
                        type = Enum.MessageType.alert_message
                    });
                }
            }
        }

        [NonAction]
        public async Task PushNotice(NotifyMessage message, List<long> userIds)
        {
            var list =await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user where UserId in @list", new { list = userIds });
            var userList = list.ToList();
            if (userList.Any())
            {
                foreach (var item in userList)
                {
                    await _userHubContext.Clients.Client(item.ConnectionId).AppendNotice(message);
                }
            }

        }

    }
}

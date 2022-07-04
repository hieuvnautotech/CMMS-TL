using CCMS.Application.Core;
using CCMS.Application.Dtos.notice;
using CCMS.Application.Services;
using CCMS.Application.Services.OnlineUser;
using CCMS.Application.Utils;
using Dapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Hubs
{
    public interface IUserClient
    {
        Task ForceExist(string str);
        Task AppendNotice(NotifyMessage message);
    }
    public class UserHub : Hub<IUserClient>
    {
        private readonly ISysCacheService _cache;
        private readonly IDapperRepository<OnlineUser> _sysOnlineUser;  // Online users 


        public UserHub(ISysCacheService cache, IDapperRepository<OnlineUser> sysOnlineUser)
        {
            _sysOnlineUser = sysOnlineUser;
            _cache = cache;
        }

        /// <summary>
        ///user  login webapp 
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"];
            var claims = JWTEncryption.ReadJwtToken(token)?.Claims;
            var userId = claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_USERID)?.Value;
            var account = claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_ACCOUNT)?.Value;
            var name = claims.FirstOrDefault(e => e.Type == ClaimConst.CLAINM_NAME)?.Value;
           
            var ip = HttpNewUtil.Ip;
            
                var list = await _sysOnlineUser.QueryAsync<OnlineUser>(@"select * from sys_online_user where Account=@account and LastLoginIp=@ip", new { account, ip });
                var userlist = list.ToList();
                if (userlist.Count > 0)
                {
                    
                    await _sysOnlineUser.ExecuteAsync(@"delete from sys_online_user where Account=@account and LastLoginIp=@ip", new { account, ip });

                }

                OnlineUser user = new OnlineUser()
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = long.Parse(userId),
                    LastTime = DateTime.Now,
                    LastLoginIp = ip,
                    Account = account,
                    Name = name,

                };
                await _sysOnlineUser.InsertAsync(user);
            
           
        }

        /// <summary>
        /// disconnect
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (!string.IsNullOrEmpty(Context.ConnectionId))
            {

                await _sysOnlineUser.ExecuteAsync(@"delete from sys_online_user where Connectionid=@connectionId and isnull(IsAccount_Locked_By_Ip,0)=0 and isnull(Is_Account_Locked,0)=0", new { connectionid= Context.ConnectionId });
  
            }
        }

    }
}


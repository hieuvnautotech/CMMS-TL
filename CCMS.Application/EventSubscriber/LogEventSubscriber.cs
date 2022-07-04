using System;
using System.Threading.Tasks;
using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Auth;
using Dapper;
using Furion.EventBus;
 
using Microsoft.Extensions.DependencyInjection;
 

namespace CCMS.Application.EventSubscriber
{
    public class LogEventSubscriber : IEventSubscriber
    {
        public LogEventSubscriber(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        //[EventSubscribe("Create:OpLog")]
        //public async Task CreateOpLog(EventHandlerExecutingContext context)
        //{
        //    using var scope = Services.CreateScope();
        //    var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogOp>>();
        //    var log = (SysLogOp)context.Source.Payload;
        //    await _repository.InsertAsync(log);
        //}

        //[EventSubscribe("Create:ExLog")]
        //public async Task CreateExLog(EventHandlerExecutingContext context)
        //{
        //    using var scope = Services.CreateScope();
        //    var _repository = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<SysLogEx>>();
        //    var log = (SysLogEx)context.Source.Payload;
        //    await _repository.InsertAsync(log);
        //}

        [EventSubscribe("Create:VisLog")]
        public async Task CreateVisLog(EventHandlerExecutingContext context)
        {
            try
            {
                using var scope = Services.CreateScope();
                var _repository = scope.ServiceProvider.GetRequiredService<IDapperRepository<sys_log_vis>>();
                var log = (sys_log_vis)context.Source.Payload;

                await _repository.InsertAsync(log);
            } catch (Exception ex)
            {

            }
           
        }

        [EventSubscribe("Update:UserLoginInfo")]
        public async Task UpdateUserLoginInfo(EventHandlerExecutingContext context)
        {
            using var scope = Services.CreateScope();
            var _repository = scope.ServiceProvider.GetRequiredService<IDapperRepository>();
            var log = (userDto)context.Source.Payload;
            await _repository.ExecuteAsync(@"update sys_user 
set LastLoginIp=@LastLoginIp, LastLoginTime=@LastLoginTime ", new {log.LastLoginIp, log.LastLoginTime });
               
        }
    }
}
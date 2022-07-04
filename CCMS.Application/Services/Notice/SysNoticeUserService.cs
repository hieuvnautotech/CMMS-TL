

using CCMS.Application.Dtos;
using CCMS.Application.Dtos.notice;
using CCMS.Application.Enum;
using Dapper;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.Application.Services.Notice
{
    public interface ISysNoticeUserService
    {
        Task Add(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus);

        Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId);

        Task Read(long noticeId, long userId, NoticeUserStatus status);

        Task Update(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus);
    }
    public class SysNoticeUserService : ISysNoticeUserService, ITransient
    {
        private readonly IDapperRepository<SysNoticeUser> _sysNoticeUserRep; 
      
        public SysNoticeUserService(IDapperRepository<SysNoticeUser> sysNoticeUserRep)
        {
            _sysNoticeUserRep = sysNoticeUserRep;
        }

      
        public async Task Add(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus)
        {
            List<SysNoticeUser> list = new List<SysNoticeUser>();
            noticeUserIdList.ForEach(u =>
            {
                list.Add(new SysNoticeUser
                {
                    NoticeId = noticeId,
                    UserId = u,
                    ReadStatus = noticeUserStatus
                });
            });
            await _sysNoticeUserRep.InsertAsync(list);
        }

       
        public async Task Update(long noticeId, List<long> noticeUserIdList, NoticeUserStatus noticeUserStatus)
        {
            await _sysNoticeUserRep.Context.ExecuteAsync(@"delete from sys_notice_user where noticeid=@noticeId", new { noticeId }) ;

            await Add(noticeId, noticeUserIdList, noticeUserStatus);
        }

       
        public async Task<List<SysNoticeUser>> GetNoticeUserListByNoticeId(long noticeId)
        {
            var list= await _sysNoticeUserRep.QueryAsync< SysNoticeUser>(@"select * from sys_notice_user where NoticeId=@noticeId", new { noticeId });//.Where(u => u.NoticeId == noticeId).ToListAsync();
            return list.ToList();
        }

       
        public async Task Read(long noticeId, long userId, NoticeUserStatus status)
        {
            await _sysNoticeUserRep.Context.ExecuteAsync(@"
update sys_notice_user set  ReadStatus=@status, 
ReadTime=@ReadTime
where NoticeId = @noticeId and UserId = @userId",
new { status, ReadTime = DateTime.Now });
           
        }
    }
}

using CCMS.Application.Dtos.notice;
using CCMS.Application.Enum;
using CCMS.Application.Services.OnlineUser;
using CCMS.Application.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services.Notice
{


    public class ChangeStatusNoticeInput : DeleteNoticeInput
    {
        /// <summary>
        /// Status (dictionary 0 draft 1 published 2 withdrawn 3 deleted)
        /// </summary>
        [Required(ErrorMessage = "The state cannot be empty")]
        public NoticeStatus Status { get; set; }
    }

    [ApiDescriptionSettings(Name = "Notice", Order = 500)]
    public class SysNoticeService: IDynamicApiController, ITransient
    {
        //public async Task<dynamic> QueryNoticePageList([FromQuery] NoticeInput input)
        //{
        //    var notices = await _sysNoticeRep.Context.Queryable<SysNotice>()
        //                                    .WhereIF(!string.IsNullOrWhiteSpace(input.SearchValue), u => u.Title.Contains(input.SearchValue.Trim()) || u.Content.Contains(input.SearchValue.Trim()))
        //                                    .WhereIF(input.Type > 0, u => u.Type == input.Type)
        //                                    .Where(u => u.Status != NoticeStatus.DELETED)
        //                                    .ToPagedListAsync(input.PageNo, input.PageSize);
        //    return notices.XnPagedResult();
        //}
        private readonly IDapperRepository _dapper;
        private readonly ISysNoticeUserService _sysNoticeUserService;
        private readonly ISysOnlineUserService _sysOnlineUserService;

        public SysNoticeService(IDapperRepository dapperRepository, ISysOnlineUserService sysOnlineUserService, ISysNoticeUserService sysNoticeUserService)
        {
            _dapper = dapperRepository;
            _sysNoticeUserService = sysNoticeUserService;
            _sysOnlineUserService = sysOnlineUserService;
        }
        [HttpPost("/sysNotice/add")]
        [AllowAnonymous]
        public async Task AddNotice(AddNoticeInput input)
        {
            if (input.Status != NoticeStatus.DRAFT && input.Status != NoticeStatus.PUBLIC)
                throw Oops.Oh(ErrorCode.D7000);

            var notice = input.Adapt<SysNotice>();
              UpdatePublicInfo(notice);
            //If it is a release, set the release time
            if (input.Status == NoticeStatus.PUBLIC)
                notice.PublicTime = DateTime.Now;

            notice.CreatedUserId  =(int)UserManager.UserId;
            notice.CreatedUserName = UserManager.Name;
          

        var id = await _dapper.Context.ExecuteScalarAsync<long>(@"
                                INSERT INTO [dbo].[sys_notice]
                                           ([Title]
                                           ,[Content]
                                           ,[PublicUserId]
                                           ,[PublicUserName]
                                           ,[PublicOrgId]
                                           ,[PublicOrgName]
                                           ,[CreatedTime]
                                           ,[CreatedUserName]
                                           ,[CreatedUserId]
                                           ,[PublicTime]
                                            ,[Type])
                                     VALUES
                                           (@Title
                                           ,@Content
                                           ,@PublicUserId
                                           ,@PublicUserName
                                           ,null
                                           ,null
                                           ,getdate()
                                           ,@CreatedUserName
                                           ,@CreatedUserId
                                           ,@PublicTime
                                            ,@Type)
                                SELECT CAST(SCOPE_IDENTITY() as bigint)
                                ", notice);

            // Person to notify
            var noticeUserIdList = input.NoticeUserIdList;
            var noticeUserStatus = NoticeUserStatus.UNREAD;
            await _sysNoticeUserService.Add(id, noticeUserIdList, noticeUserStatus);

            if (input.Status == NoticeStatus.PUBLIC)
            {
                await _sysOnlineUserService.PushNotice(new NotifyMessage
                {
                      type= MessageType.alert_message,
                    data = new {title= input.Title, content= input .Content}
                }, noticeUserIdList);
            }

        }

        /// <summary>
        /// Update release information
        /// </summary>
        /// <param name="notice"></param>
        [NonAction]
        private  void  UpdatePublicInfo(SysNotice notice)
        {
           // var emp = await _sysEmpRep.FirstOrDefaultAsync(u => u.Id == UserManager.UserId);
            notice.PublicUserId = UserManager.UserId;
            notice.PublicUserName = UserManager.Name;
          //  notice.PublicOrgId = emp.OrgId;
           // notice.PublicOrgName = emp.OrgName;
        }



        /// <summary>
        ///unprocessed message
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("/sysNotice/unread")]
       
        public async Task<dynamic> UnReadNoticeList([FromQuery] NoticeInput input)
        {
            var dic = typeof(NoticeType).EnumToList();
            var notices = _dapper.Context.Query<NoticeReceiveOutput>(@"
select top(20) * from Sys_Notice a
left join Sys_Notice_User b on b.NoticeId=a.id
where b.Userid=@userid and b.ReadStatus=@readstatus
order by a.CreatedTime desc

", new { userid = UserManager.UserId, readstatus = NoticeUserStatus.UNREAD });
           
           // var count = await _dapper.Context.Queryable<SysNotice, SysNoticeUser>((n, u) => new JoinQueryInfos(JoinType.Inner, n.Id == u.NoticeId)).Where((n, u) => u.UserId == UserManager.UserId && u.ReadStatus == NoticeUserStatus.UNREAD).CountAsync();

           var noticeClays = new List<dynamic>();
            int index = 0;
            foreach (var item in dic)
            {
                noticeClays.Add(
                    new
                    {
                        Index = index++,
                        Key = item.Describe,
                        Value = item.Value,
                        NoticeData = notices.Where(m => m.Type == item.Value).ToList()
                    }
                );
            }
            return noticeClays;
        }

    }
}

using CCMS.Application.Dtos.notice;
using CCMS.Application.Services.OnlineUser;
using CCMS.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services.Notice
{
    public interface ISysNotifyUploadService {
        Task PushNotify_UploadedFile(NotifyMessage message);

    }
    public class SysNotifyUploadService: ISysNotifyUploadService, ITransient
    {
        private readonly ISysOnlineUserService _sysOnlineUserService;

        public SysNotifyUploadService(ISysOnlineUserService sysOnlineUserService)
        {
            _sysOnlineUserService = sysOnlineUserService;
        }

        public async Task PushNotify_UploadedFile(NotifyMessage message)
        {
            var alluseronline=await _sysOnlineUserService.GetAllOnlineUser();
            await _sysOnlineUserService.PushNotice(message, alluseronline);
        }
    }
}

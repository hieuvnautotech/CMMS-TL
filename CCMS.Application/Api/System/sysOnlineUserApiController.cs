using CCMS.Application.Dtos;
using CCMS.Application.Services;
using CCMS.Application.Services.OnlineUser;
using CCMS.Application.Utils;
using CCMS.Core.Filter;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("System",Tag = "Quản lý user online")]
    
    public class sysOnlineUserApiController : ControllerBase
    {
        private readonly IDapperRepository _dapper;

        private readonly ISysOnlineUserService _userSvr;
        public sysOnlineUserApiController(IDapperRepository dapper, ISysOnlineUserService userSvr, ISysCacheService ss)
        {
            _userSvr = userSvr;
            _dapper = dapper;
        }

        //gửi thông điệp đến user, nếu chỉnh định ip thì chỉ máy có ip mới nhận thông điệp
        [HttpPost("send-message-user")]
        public async Task< IActionResult> SendMessageToUser([FromBody] UserOnline_Model input)
        {
            await _userSvr.SendMessage(input.Message, input.UserId.Value, input.Ip);
            return Ok();
        }

        //gửi thông điệp đến tất cả user
        [HttpPost("send-message-all")]
        public async Task<IActionResult> SendMessageAll([FromBody] UserOnline_Model input)
        {
            await _userSvr.SendMessageAll(input.Message);
            return Ok();
        }

        //bắt buộc user logout khỏi ứng dụng, user có thể đăng nhập lại
        [HttpPost("force-exit")]
        public async Task<IActionResult> ForceExit([FromBody] UserOnline_Model input)
        {
            await _userSvr.ForceExit(input.UserId.Value,input.Ip);
            return Ok();
        }

        //bắt buộc logout khỏi ứng dụng nếu user đang online, user không thể đăng nhập lại 
        //nếu chỉ định ip thì máy có ip bị lock, ngược lại lock account
        [HttpPost("lock-account")]
        public async Task<IActionResult> LockAccount([FromBody] UserOnline_Model input)
        {
            await _userSvr.LockAccount(input.UserId.Value, input.Ip);
            return Ok();
        }

        [HttpGet("get-list-online-user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            var ip = HttpNewUtil.Ip;
           var list= await _userSvr.GetList();
            return Ok(list.Where(a=>a.LastLoginIp != ip).ToList());
        }



    }
}

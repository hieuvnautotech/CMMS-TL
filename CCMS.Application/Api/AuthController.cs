using CCMS.Application.Core;
using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Auth;
using CCMS.Application.Enum;
using CCMS.Application.Services;
using CCMS.Application.Services.OnlineUser;
using CCMS.Application.Utils;
using Dapper;
using Furion.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAParser;

namespace CCMS.Application.Api
{
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Auth",Tag = "Login, đăng nhập")]
    public class AuthController: ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMenuService _menuSvr;
        private readonly IUserService _userSvr;
        private readonly ISysOnlineUserService _sysOnlineUser;
        public AuthController(IHttpContextAccessor httpContextAccessor,
             IEventPublisher eventPublisher,
             ISysOnlineUserService sysOnlineUser,
             IMenuService menuSvr,
            IUserService userSvr)
        {
            _sysOnlineUser = sysOnlineUser;
            _userSvr = userSvr;
            _menuSvr = menuSvr;

            _httpContextAccessor = httpContextAccessor;
            _eventPublisher = eventPublisher;
        }
     
        /// <summary>
        /// Log in vào tài khoản để truy cập dữ liệu trên server
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<string> Login([Required][FromBody] LoginInput input)
        {
            
            // Get encrypted password
            var encryptPassword = MD5Encryption.Encrypt(input.Password);

            // Determine if username and password are correct 
            var user = await _userSvr.checkLogin(input.Account, encryptPassword);

            var roles = _userSvr.GetUserRoleList(user.Id);
            // Generate Token
            //var accessToken = await _jwtBearerManager.CreateTokenAdmin(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                {ClaimConst.CLAINM_USERID, user.Id},
                {ClaimConst.CLAINM_ACCOUNT, user.Account},
                {ClaimConst.CLAINM_NAME, user.FullName},
                {ClaimConst.CLAINM_SUPERADMIN, user.AdminType},
                 {ClaimConst.CLAINM_ROLES, string.Join(",",roles.Select(a=>a.Name))},
            });

            // Set up Swagger auto-login
            _httpContextAccessor.HttpContext.SigninToSwagger(accessToken);

            // Generate refresh token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 30);

            // Set refresh token token
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            //update useinfo
            var httpContext = App.HttpContext;
            await _eventPublisher.PublishAsync(new ChannelEventSource("Update:UserLoginInfo",
                new userDto { Id = user.Id, LastLoginIp = httpContext.GetLocalIpAddressToIPv4(), 
                    LastLoginTime = DateTime.Now }));

            var ip = HttpNewUtil.Ip;
            //create log
            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new sys_log_vis
                {
                    Name = user.FullName,
                    Success = YesOrNot.Y,
                    Message = "Log in",

                    VisTime = DateTime.Now,
                    Account = user.Account,
                    Ip = ip
                }));
            return accessToken;
        }

        /// <summary>
        /// Get currently logged in user information
        /// </summary>
        /// <returns></returns>
        [HttpGet("getLoginUser")]
        public async Task< LoginOutput> GetLoginUser()
        {
            var user = _userSvr.Get_User_by_Id(UserManager.UserId);
            var userId = user.Id;

            var httpContext = App.GetService<IHttpContextAccessor>().HttpContext;
            var loginOutput = user.Adapt<LoginOutput>();

            loginOutput.LastLoginTime = user.LastLoginTime = DateTime.Now;
            var ip = HttpNewUtil.Ip;

            var list = await _sysOnlineUser.GetOnlineUser(userId);
            if (list.Any(a => a.Is_Account_Locked == true))
            {
                //account is locked
                throw Oops.Oh(ErrorCode.D1005);
            } else if (list.Any(a => a.IsAccount_Locked_By_Ip == true && a.LastLoginIp==ip))
            {
                throw Oops.Oh(ErrorCode.D1006);
            }


                loginOutput.LastLoginIp = user.LastLoginIp =
                string.IsNullOrEmpty(user.LastLoginIp) ? HttpNewUtil.Ip : ip;

            var clent = Parser.GetDefault().Parse(httpContext.Request.Headers["User-Agent"]);
            loginOutput.LastLoginBrowser = clent.UA.Family + clent.UA.Major;
            loginOutput.LastLoginOs = clent.OS.Family + clent.OS.Major;

            loginOutput.Roles =  _userSvr.GetUserRoleList(userId);


            // menu info
            loginOutput.Menus = await _menuSvr.GetLoginMenusDesign(userId);
               
            return loginOutput;
        }

        /// <summary>
        /// Thoát đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [AllowAnonymous]
        public async Task LogoutAsync()
        {
            _httpContextAccessor.HttpContext.SignoutToSwagger();
            var ip = HttpNewUtil.Ip;
            var user = _userSvr.Get_User_by_Id(UserManager.UserId);

            await _eventPublisher.PublishAsync(new ChannelEventSource("Create:VisLog",
                new sys_log_vis
                {
                    Name = user.FullName,
                    Success = YesOrNot.Y,
                    Message = "Log out",
                   
                    VisTime = DateTime.Now,
                    Account = user.Account,
                    Ip = ip
                }));
            await Task.CompletedTask;
        }

    }
}

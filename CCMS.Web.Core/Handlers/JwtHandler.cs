using CCMS.Application.Core;
using CCMS.Application.Enum;
using CCMS.Application.Options;
using CCMS.Application.Services;
using CCMS.Application.Utils;
using CCMS.Core.Filter;
using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTSettingsOptions = CCMS.Application.Options.JWTSettingsOptions;

namespace CCMS.Web.Core
{
    public class JwtHandler : AppAuthorizeHandler
    {
       
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
           
            // Auto refresh token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext(),
                App.GetOptions<JWTSettingsOptions>().ExpiredTime,
                App.GetOptions<RefreshTokenSettingOptions>().ExpiredTime))
            {
                await AuthorizeHandleAsync(context);
            }
            else
            {
                context.Fail(); 
                DefaultHttpContext currentHttpContext = context.GetCurrentHttpContext();
                if (currentHttpContext == null)
                    return;
                currentHttpContext.SignoutToSwagger();
            }
        }
      
        /// <summary>
        /// Kiểm tra logic, trả về true nếu được uỷ quyền, ngược lại false
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override  Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
           
            //check roles neu co, lay thong tin AuthorizeRoleAttribute neu co trong action
            var actionDescriptor = httpContext.GetControllerActionDescriptor();
            var method = actionDescriptor.MethodInfo;
            var attribute = (method.GetCustomAttributes(typeof(AuthorizeRoleAttribute), true).FirstOrDefault() as AuthorizeRoleAttribute);
            if (attribute != null)
            {
                var allow_roles = attribute.Roles?.Split(",");
                //kie63m tra xem co bat ki trong token
                var roles_current_user_login = UserManager.Roles?.Split(",");
                bool hasCommonRoles = allow_roles.Intersect(roles_current_user_login, StringComparer.OrdinalIgnoreCase).Count() > 0;
                if (hasCommonRoles) return Task.FromResult(true);
            }
            // Administrator skip check
            if (UserManager.IsSuperAdmin) return Task.FromResult(true);

            // Token JWT đã được , kiểm tra phân quyền custom theo permission trong database
            return CheckPermissions(httpContext);
        }

    
        private  async Task< bool> CheckPermissions(DefaultHttpContext httpContext)
        {
            // Route name
            var routeName = httpContext.Request.Path.Value[1..].Replace("/", ":");

            // Default route (get login user )
            var defalutRoute = new List<string>()
            {
                "api:Auth:getLoginUser",    //Log in
               // "sys Menu:change" //Switch the top menu
            };

            if (defalutRoute.Contains(routeName)) return true;

            
            // Get user permissions
            var allPermission = await App.GetService<IMenuService>().GetAllPermission();
            if (!allPermission.Contains(routeName, StringComparer.OrdinalIgnoreCase))
            {
                // If there is no configuration button permission in the menu, auth false
                return false;
            }

            var permissionList = await App.GetService<IMenuService>().GetLoginPermissionList(UserManager.UserId);
            // check authorization
            return permissionList.Contains(routeName);


        }
    }
}
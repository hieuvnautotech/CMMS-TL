using CCMS.Application.Core;
using Furion.RemoteRequest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UAParser;

namespace CCMS.Application.Utils
{
    public static class HttpNewUtil
    {
        
        public static string Ip
        {
            get
            {
                var result = string.Empty;
                if (App.HttpContext != null)
                {
                    result = GetWebClientIp();
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = GetLanIp();
                }

                return result;
            }
        }

       
        private static string GetWebClientIp()
        {
            var ip = GetWebRemoteIp();
            foreach (var hostAddress in Dns.GetHostAddresses(ip))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            return string.Empty;
        }

       
        public static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            return string.Empty;
        }

       
        private static string GetWebRemoteIp()
        {
            if (App.HttpContext?.Connection?.RemoteIpAddress == null)
                return string.Empty;
            var ip = App.HttpContext?.Connection?.RemoteIpAddress.ToString();
            if (App.HttpContext == null)
                return ip;
            if (App.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ip = App.HttpContext.Request.Headers["X-Real-IP"].ToString();
            }

            if (App.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = App.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            }

            return ip;
        }

         
        public static string UserAgent
        {
            get
            {
                string userAgent = App.HttpContext?.Request?.Headers["User-Agent"];

                return userAgent;
            }
        }

     
        public static string Url
        {
            get
            {
                var url = new StringBuilder().Append(App.HttpContext?.Request?.Scheme).Append("://")
                    .Append(App.HttpContext?.Request?.Host).Append(App.HttpContext?.Request?.PathBase)
                    .Append(App.HttpContext?.Request?.Path).Append(App.HttpContext?.Request?.QueryString).ToString();
                return url;
            }
        }

       
        public static string GetOSVersion()
        {
            var osVersion = string.Empty;
            var userAgent = UserAgent;
            if (userAgent.Contains("NT 10"))
            {
                osVersion = "Windows 10";
            }
            else if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows 8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows 7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "Windows Vista/Server 2008";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "Windows Server 2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "Windows XP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows 2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "Windows NT4";
            }
            else if (userAgent.Contains("Android"))
            {
                osVersion = "Android";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "Windows Me";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows 95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }

            return osVersion;
        }

        
      
       
        public static UserAgentInfoModel UserAgentInfo()
        {
            var parser = Parser.GetDefault();
            var clientInfo = parser.Parse(UserAgent);
            var result = new UserAgentInfoModel
            {
                PhoneModel = clientInfo.Device.ToString(),
                OS = clientInfo.OS.ToString(),
                Browser = clientInfo.UA.ToString()
            };
            return result;
        }

        private static readonly char[] reserveChar = { '/', '?', '*', ':', '|', '\\', '<', '>', '\"' };

       
        public static string EncodeRemotePath(string remotePath)
        {
            if (remotePath == "/")
            {
                return remotePath;
            }

            var endWith = remotePath.EndsWith("/");
            var part = remotePath.Split('/');
            remotePath = "";
            foreach (var s in part)
            {
                if (s == "")
                    continue;
                if (remotePath != "")
                {
                    remotePath += "/";
                }

                remotePath += HttpUtility.UrlEncode(s).Replace("+", "%20");
            }

            remotePath = (remotePath.StartsWith("/") ? "" : "/") + remotePath + (endWith ? "/" : "");
            return remotePath;
        }

        public static string StandardizationRemotePath(string remotePath)
        {
            if (string.IsNullOrEmpty(remotePath))
            {
                return "";
            }

            if (!remotePath.StartsWith("/"))
            {
                remotePath = "/" + remotePath;
            }

            if (!remotePath.EndsWith("/"))
            {
                remotePath = remotePath + "/";
            }

            var index1 = 1;
            while (index1 < remotePath.Length)
            {
                var index2 = remotePath.IndexOf('/', index1);
                if (index2 == index1)
                {
                    return "";
                }

                var folderName = remotePath.Substring(index1, index2 - index1);
                if (folderName.IndexOfAny(reserveChar) != -1)
                {
                    return "";
                }

                index1 = index2 + 1;
            }

            return remotePath;
        }
    }

   
   
    public class UserAgentInfoModel
    {
        
        public string PhoneModel { get; set; }

        
        public string OS { get; set; }

        
        public string Browser { get; set; }
    }
}

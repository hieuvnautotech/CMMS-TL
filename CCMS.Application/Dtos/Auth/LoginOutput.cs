using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Auth
{
    public class LoginOutput
    {
       
        public long Id { get; set; }

       
        public string Account { get; set; }

      
        public string FullName { get; set; }

      
      
        public int AdminType { get; set; }

        public string LastLoginIp { get; set; }

       
        public DateTime LastLoginTime { get; set; }

     
        public string LastLoginAddress { get; set; }

      
        public string LastLoginBrowser { get; set; }

   
       
       
        public List<RoleOutput> Roles { get; set; } = new List<RoleOutput>();

      
        public List<string> Permissions { get; set; } = new List<string>();

      
       public List<MenuDesignTreeNode> Menus { get; set; } = new List<MenuDesignTreeNode>();
       public string LastLoginOs { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Core.Filter
{
    public class AuthorizeRoleAttribute:Attribute
    {
        public string Roles { get; set; }
       public AuthorizeRoleAttribute(params string[] roles)
        {
            Roles=string.Join(",", roles);
        }
    }
}

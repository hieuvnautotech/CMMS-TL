using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Options
{
    public class JWTSettingsOptions : IConfigurableOptions
    {
        
        public bool ValidateIssuerSigningKey { get; set; }
       
        public string IssuerSigningKey { get; set; }
        
        public bool ValidateIssuer { get; set; }
        
        public string ValidIssuer { get; set; }
        
        public bool ValidateAudience { get; set; }
       
        public string ValidAudience { get; set; }
       
        public bool ValidateLifetime { get; set; }
        
        public long ExpiredTime { get; set; }
       
        public long ClockSkew { get; set; }
    }
}

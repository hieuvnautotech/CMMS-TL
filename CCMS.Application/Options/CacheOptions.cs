using CCMS.Application.Enum;
using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Options
{
    public class CacheOptions : IConfigurableOptions
    {
        
        public CacheType CacheType { get; set; }

       
        public string RedisConnectionString { get; set; }
    }
}

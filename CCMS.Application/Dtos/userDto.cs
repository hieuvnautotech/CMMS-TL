using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class userDto
    {
        public int Id { get; set; }
        public string Account { get; set; }

        public string FullName { get; set; }

        public int? AdminType { get; set; }

        public DateTime LastLoginTime { get; set; }

        public string LastLoginIp { get; set; }

    }
}

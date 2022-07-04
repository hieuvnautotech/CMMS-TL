using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.StandardDB
{
    public class UserAccountModel
    {
        public int? userid { get; set; }
        [Required(ErrorMessage = "Required user name")]
        public string username { get; set; }
        public string password { get; set; }
        public bool? active { get; set; }
        [Required(ErrorMessage = "Required staff")]
        public int? staffid { get; set; }
        public string staffname { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos.Auth
{
    public class LoginInput
    {
        
        [Required(ErrorMessage = "Username can not be empty"), MinLength(3, ErrorMessage = "Username cannot be less than 3 characters")]
        public string Account { get; set; }

      
        [Required(ErrorMessage = "password can not be blank"), MinLength(5, ErrorMessage = "Password must not be less than 5 characters")]
        public string Password { get; set; }
    }
}

using CCMS.Application.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class Add_Menu_input: IValidatableObject
    {
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]

        public string Code { get; set; }
        [Required]
       
        public string Router { get; set; }
        [Required]
        public string Component { get; set; }
        public virtual long? Pid { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           // var service = validationContext.GetService(typeof(type));
            if (Name.Length <5)
            {
                yield return new ValidationResult(
                    "Error name is too short"
                    , new[] { nameof(Name) }
                );
            }
        }


   

    }
    public class Add_Permission_input2
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }


        [Required]
        public string Component { get; set; }

        [Required]
        public string Router { get; set; }


      //  [Required(ErrorMessage = "Vui long chon menu cha")]
       // public MenuLocation? Type { get; set; }


    }

    public class Add_Permission_input
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }


        [Required]
        public  long? Pid { get; set; }


        // public virtual  MenuType? Type { get; set; }

    }


    public class UpdateMenuInput : Add_Menu_input
    {
        
        [Required(ErrorMessage = "The menu ID cannot be empty")]
        public long Id { get; set; }

      
       // [Required(ErrorMessage = "The parent menu ID cannot be empty")]
        public override long? Pid { get; set; }

        [Required(ErrorMessage = "Vui long chon type")]
        [Description("0: folder, 1:menu, 2: permission")]
        public  MenuType? Type { get; set; }
    }
    public class GetMenuListInput
    {
       
        public string Name { get; set; }

       
    }


}

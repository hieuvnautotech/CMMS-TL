using CCMS.Application.Enum;
using CCMS.Application.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class MenuOutput : ITreeNode
    {
        
        public long Id { get; set; }

      
        public List<MenuOutput> Children { get; set; } = new List<MenuOutput>();

      
        public long Pid { get; set; }

     
        public string Name { get; set; }

        public string Code { get; set; }

      
        public MenuType Type { get; set; }

        public string Icon { get; set; }

        public string Router { get; set; }

   
        public string Component { get; set; }

      
        public string Permission { get; set; }

        public string Application { get; set; }

        public string Visible { get; set; }

  
        public int Sort { get; set; }

        public long GetId()
        {
            return Id;
        }

        public long GetPid()
        {
            return Pid;
        }

        public void SetChildren(IList children)
        {
            Children = (List<MenuOutput>)children;
        }
    }
}

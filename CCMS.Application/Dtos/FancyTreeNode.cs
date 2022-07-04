using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Dtos
{
    public class FancyTreeNode
    {
        public string title { get; set; }
        public bool folder { get; set; }
        public bool expanded { get; set; }
        public int nodeid { get; set; }
        public bool lazyload { get; set; }
        public bool isroot { get; set; }
        public string extrainfo { get; set; }
        public List<FancyTreeNode> children { get; set; }
    }

    public class FancyTreeNode2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string disabled { get; set; }
        public List<FancyTreeNode2> children { get; set; }


    }

    public class FancyTreeNode3
    {
        public string title { get; set; }
        public bool folder { get; set; }
        public bool expanded { get; set; }
        public string nodeid { get; set; }
        public bool lazy { get; set; }
        public bool isroot { get; set; }
        public string extrainfo { get; set; }
        public List<FancyTreeNode> children { get; set; }
    }
}

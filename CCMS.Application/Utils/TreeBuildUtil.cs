using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Utils
{
    public interface ITreeNode
    {
       
        long GetId();

        
        long GetPid();

        
        void SetChildren(IList children);
    }


    public class TreeBuildUtil<T> where T : ITreeNode
    {
       
        private long _rootParentId = 0L;

        
        public void SetRootParentId(long rootParentId)
        {
            _rootParentId = rootParentId;
        }

     
        public List<T> Build(List<T> nodes)
        {
            nodes.ForEach(u => BuildChildNodes(nodes, u, new List<T>()));

            var result = new List<T>();
            nodes.ForEach(u =>
            {
                if (_rootParentId == u.GetPid())
                    result.Add(u);
            });
            return result;
        }

     
        private void BuildChildNodes(List<T> totalNodes, T node, List<T> childNodeList)
        {
            var nodeSubList = new List<T>();
            totalNodes.ForEach(u =>
            {
                if (u.GetPid().Equals(node.GetId()))
                    nodeSubList.Add(u);
            });
            nodeSubList.ForEach(u => BuildChildNodes(totalNodes, u, new List<T>()));
            childNodeList.AddRange(nodeSubList);
            node.SetChildren(childNodeList);
        }
    }
}

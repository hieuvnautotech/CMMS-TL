using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Auth;
using CCMS.Application.Dtos.EquipmentManager;
using CCMS.Application.Enum;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Services
{
    public interface IEquipmentManagerService
    {
        List<FancyTreeNode> GetTreeParts();
        Part_Model_Single GetSinglePart(int? part_id);

    }
    public class EquipmentManagerService : IEquipmentManagerService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public EquipmentManagerService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }


        public Part_Model_Single GetSinglePart(int? part_id)
        {


            var part = _dapper.Context.QueryFirstOrDefault<Part_Model_Single>(@"
SELECT a.*, b.location_name, c.type_name,d.manufacturer_name, f.part_name parent_name FROM SD_Part a
LEFT  JOIN SD_Location b ON b.location_id=a.location_id
LEFT JOIN SD_Type c ON c.type_id=a.type_id
LEFT JOIN SD_Manufacturer d ON d.manufacturer_id=a.manufacturer_id
LEFT JOIN SD_Part f ON f.part_id = a.parent_id
where a.part_id=@part_id
", new { part_id });


                return part;

        }

        public List<FancyTreeNode> GetTreeParts()
        {
            var list = _dapper.Context.Query<Part_Model>(@"
select a.* from [SD_Part] a
        where a.[use] =1 or a.[use] is null",
                new {  }, commandType: CommandType.Text).ToList();
            var dictParts = list.ToDictionary(a => a.part_id, a=>a);
            var arr = dictParts.Values.ToArray();
            var lstFound = new List<FancyTreeNode>();

            foreach (var d in arr.Where(a => a.parent_id == null))
            {
                lstFound.Add(new FancyTreeNode
                {
                    title = d.part_name,
                    expanded = false,
                    nodeid = d.part_id,
                    extrainfo = "part"
                });

                getAllChildrenDept(lstFound.Last(), dictParts, 0);

            }

         return   lstFound;
        }



        private void getAllChildrenDept(FancyTreeNode ParentNode, Dictionary<int, Part_Model> dictParts, int deep)
        {

            var ParentId = ParentNode.nodeid;
            dictParts.Remove(ParentId);
            var q = dictParts.Values.Where(a => a.parent_id == ParentId).ToArray();

            if (q == null || q.Length == 0) return;

            ParentNode.folder = true;
            var lstNodes = new List<FancyTreeNode>();
            foreach (var w in q.OrderBy(a => a.part_name))
            {

                var node = new FancyTreeNode
                {
                    title = w.part_name,
                    expanded = true,
                    nodeid = w.part_id,
                    extrainfo = "part"
                };
                lstNodes.Add(node);
                getAllChildrenDept(node, dictParts, deep + 1);
            }

            ParentNode.children = lstNodes;

        }



    }
}

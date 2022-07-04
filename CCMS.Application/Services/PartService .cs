using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Auth;
using CCMS.Application.Enum;
using CCMS.Application.Utils;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;
using CCMS.Application.Dtos.StandardDB;

namespace CCMS.Application.Services
{
    public interface IPartService
    {
        Task<IEnumerable<Part_Model_GetList>> GetAllSearchPart(string search_name);
        Task<int> insertPartInfo(Part_Input input);
        Task<int> UpdatePartInfo(Part_Input input);
        Task<int> DeletePartInfo(int tool_id);



    }
    public class PartService : IPartService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public PartService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }
        public async Task<IEnumerable<Part_Model_GetList>> GetAllSearchPart(string search_name)
        {


            var query = @"select a.part_id, a.part_id as id, a.part_code,a.part_name,a.spec,FORMAT(a.price,'N') AS price2,a.price,a.created_at,a.unit,a.type_id,a.location_id,
                            a.manufacturer_id,a.remark,a.[use],b.location_name,c.manufacturer_name,d.type_name
                            from SD_Part a
                            left join SD_Location b on a.location_id=b.location_id
                            left join SD_Manufacturer c on a.manufacturer_id=c.manufacturer_id
                            left join SD_Type d on a.type_id=d.type_id
                            where 1=1 and a.[use]=1";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += "and part_name like '%' + @search_name + '%'";
            }
            var result = await _dapper.QueryAsync<Part_Model_GetList>(query, new { @search_name = search_name });
            return result;
        }
        public async Task<int> insertPartInfo(Part_Input input)
        {
            try
            {
                string sql = @" INSERT INTO [dbo].[SD_Part]
                                                               ( [part_code],[part_name] ,[spec],[use],[price],[type_id],[location_id],[manufacturer_id],[unit],[remark],[created_at])
                              VALUES
                                                                (@part_code,@part_name,@spec,@use,@price,@type_id,@location_id,@manufacturer_id,@unit,@remark,@created_at )
                              Select scope_identity()";
                int result = await _dapper.Context.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    {
                        @part_code = input.part_code,
                        @part_name = input.part_name,
                        @spec = input.spec,
                        @use = input.@use,
                        @price = input.price,
                        @type_id = input.type_id,
                        @location_id = input.location_id,
                        @manufacturer_id = input.manufacturer_id,
                        @unit = input.unit,
                        @remark = input.remark,
                        @created_at = input.created_at,
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<int> UpdatePartInfo(Part_Input input)
        {
            try
            {
                var query = @" update [dbo].[SD_Part]   set
                                                               part_name=@part_name,
                                                               spec=@spec, 
                                                                price=@price,
                                                                type_id=@type_id,
                                                                location_id=@location_id,
                                                                manufacturer_id=@manufacturer_id,
                                                                unit=@unit,
                                                                remark=@remark                                                                       
                                                    where part_id=@part_id";
                var result = await _dapper.Context.ExecuteAsync(query, new
                {
                    @part_name = input.part_name,
                    @spec = input.spec,
                    @price = input.price,
                    @type_id = input.type_id,
                    @location_id = input.location_id,
                    @manufacturer_id = input.manufacturer_id,
                    @unit = input.unit,
                    @remark = input.remark,
                    @part_id = input.part_id,

                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> DeletePartInfo(int part_id)
        {
            try
            {
                var query = @" update [dbo].[SD_Part]  set [use]='0' where part_id=@part_id";
                var result = await _dapper.Context.ExecuteAsync(query, new
                {
                    @part_id = part_id
                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<dynamic>> GetAllManufacturerId()
        {
            try
            {
                var query = @"  select 
                                manufacturer_id as value , 
                                manufacturer_name as title
                                from SD_Manufacturer a
                                where [use]=1";
                var rs = await _dapper.Context.QueryAsync<dynamic>(query);

                return rs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<dynamic>> GetAllTypeId()
        {
            try
            {
                var query = @"  select 
                                type_id as value , 
                                type_name as title
                                from SD_Type a
                                where [use]=1";
                var rs = await _dapper.Context.QueryAsync<dynamic>(query);

                return rs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
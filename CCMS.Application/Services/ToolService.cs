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
    public interface IToolService
    {
        Task<IEnumerable<ToolModel_GetList>> GetAll (string search_name);
        Task<int> insertToolInfo(ToolModel_GetList input);
        Task<int> UpdateToolInfo(ToolModel_GetList input);
        Task<int> DeleteToolInfo(int tool_id);
        Task<IEnumerable<dynamic>> GetAllManufacturerId();
        Task<IEnumerable<dynamic>> GetAllTypeId();
    }
    public class ToolService : IToolService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public ToolService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }
        public async Task<IEnumerable<ToolModel_GetList>> GetAll(string search_name)
        {


            var query = @"select a.*, m.manufacturer_name, t.type_name 
                            from dbo.SD_Tool as a 
                        join [dbo].[SD_Manufacturer] as m on a.manufacturer_id = m.manufacturer_id
                        join SD_Type as t on a.type_id = t.type_id
                            where 1=1 and a.[use] = 1 ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " and tool_name like '%' + @search_name + '%'";
            }
            var result = await _dapper.QueryAsync<ToolModel_GetList>(query, new { @search_name = search_name });
            return result;
        }
        public async Task<int> insertToolInfo(ToolModel_GetList input)
        {
            try
            {
                string sql = @" INSERT INTO [dbo].[SD_Tool]
                                                               ([tool_name]  
                                                                ,[spec]
                                                                ,[type_id]
                                                                ,[price]
                                                                ,[use]
                                                                ,[unit]
                                                                ,[manufacturer_id]
                                                               ,[created_at])
                              VALUES (@tool_name
                                                                ,@spec       
                                                                ,@type_id       
                                                                ,@price       
                                                                ,@use       
                                                                ,@unit       
                                                                ,@manufacturer_id       
                                                               ,getdate())
                              Select scope_identity()";
                int result = await _dapper.Context.QueryFirstOrDefaultAsync<int>(sql,
                    new
                    {
                        @tool_name = input.tool_name,
                        @spec = input.spec,
                        @type_id = input.type_id,
                        @price = input.@price,
                        @use = input.use,
                        @unit = input.unit,
                        @manufacturer_id = input.@manufacturer_id,
                    });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<int> UpdateToolInfo(ToolModel_GetList input)
        {
            try
            {
                var query = @" update [dbo].[SD_Tool]   set tool_name=@tool_name, spec=@spec,type_id=@type_id,price=@price,unit=@unit,manufacturer_id=@manufacturer_id where tool_id=@tool_id";
                var result = await _dapper.Context.ExecuteAsync(query, new
                {
                    @tool_name = input.tool_name,
                    @spec = input.spec,
                    @type_id = input.type_id,
                    @price = input.price,
                    @unit = input.unit,
                    @manufacturer_id = input.manufacturer_id,
                    @tool_id  = input.tool_id

                });
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<int> DeleteToolInfo(int tool_id)
        {
            try
            {
                var query = @" update [dbo].[SD_Tool]  set [use]='0' where tool_id=@tool_id";
                var result = await _dapper.Context.ExecuteAsync(query, new
                {
                    @tool_id = tool_id
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

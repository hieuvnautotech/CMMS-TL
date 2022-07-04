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
using CCMS.Application.Dtos.System;

namespace CCMS.Application.Services
{
    public interface ISysRoleService
    {
        Task<List<sysRoleModel>> GetAll(string search_name);
        Task Create(sysRoleModel model);
        Task Update(sysRoleModel model);
        Task Delete(int userid);
    }
    public class SysRoleService : ISysRoleService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public SysRoleService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        public async Task<List<sysRoleModel>> GetAll(string search_name)
        {
            var query = @"select * from sys_role ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where Name like '%' + @search_name + '%'";
            }
            var list = await _dapper.Context.QueryAsync<sysRoleModel>(query, new { search_name });

            return list.ToList();
        }

        public async Task Create(sysRoleModel model)
        {
            await _dapper.Context.ExecuteAsync(@" INSERT INTO [dbo].[sys_role] ([Name], [Remark], [Active]) VALUES (@Name, @Remark, 1) ", model);
        }

        public async Task Update(sysRoleModel model)
        {
            await _dapper.Context.ExecuteAsync(@" UPDATE [dbo].[sys_role] SET Name = @Name, Remark = @Remark, Active = @Active WHERE Id = @Id ", model);
        }

        public async Task Delete(int roleid)
        {
            await _dapper.Context.ExecuteAsync(@" DELETE from [dbo].[sys_role] WHERE Id = @roleid ", new { roleid });
        }
    }
}

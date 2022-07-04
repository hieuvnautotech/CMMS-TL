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
    public interface IUserAccountService
    {
        Task<List<UserAccountModel>> GetAll(string search_name);
        Task Create(UserAccountModel model);
        Task Update(UserAccountModel model);
        Task Delete(int userid);
        Task<IEnumerable<Dictionary<string, object>>> GetSelectStaff();
    }
    public class UserAccountService : IUserAccountService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public UserAccountService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        public async Task<List<UserAccountModel>> GetAll(string search_name)
        {
            var query = @"select a.*, b.staff_name as staffname from UserAccount a JOIN SD_Staff b on a.StaffId = b.staff_id ";
            if (!string.IsNullOrWhiteSpace(search_name))
            {
                query += " where UserName like '%' + @search_name + '%'";
            }
            var list = await _dapper.Context.QueryAsync<UserAccountModel>(query, new { search_name });

            return list.ToList();
        }

        public async Task Create(UserAccountModel model)
        {
            await _dapper.Context.ExecuteAsync(@" INSERT INTO [dbo].[UserAccount] ([UserName], [Password], [StaffId], [Active]) VALUES (@UserName, '', @StaffId, 1) ", model);
        }

        public async Task Update(UserAccountModel model)
        {
            await _dapper.Context.ExecuteAsync(@" UPDATE [dbo].[UserAccount] SET UserName = @UserName, StaffId = @StaffId, Active = @Active WHERE UserId = @UserId ", model);
        }

        public async Task Delete(int userid)
        {
            await _dapper.Context.ExecuteAsync(@" DELETE from [dbo].[UserAccount] WHERE UserId = @userid ", new { userid });
        }

        public async Task<IEnumerable<Dictionary<string, object>>> GetSelectStaff()
        {
            var list = await _dapper.Context.QueryAsync<dynamic>(@"
                                    select 
                                    staff_name as title, 
                                    staff_id as value
                                    from SD_Staff");
            return list.Select(a => new Dictionary<string, object>(a));
        }

    }
}

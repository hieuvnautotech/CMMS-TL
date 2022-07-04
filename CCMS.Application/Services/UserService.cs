using CCMS.Application.Dtos;
using CCMS.Application.Dtos.Auth;
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
    public interface IUserService
    {
        List<userDto> GetUsers();
         Task<userDto> checkLogin(string username, string password);
        userDto Get_User_by_Id(long? id);
        List<RoleOutput> GetUserRoleList(long userId);


    }
    public class UserService : IUserService, ITransient
    {
        private readonly IDapperRepository _dapper;

        public UserService(IDapperRepository dapperRepository)
        {
            _dapper = dapperRepository;
        }

        public List<userDto> GetUsers()
        {
            var data = _dapper.Query("select * from sys_user").Adapt<List<userDto>>();
            return data;
        }

        public userDto Get_User_by_Id(long? id)
        {
            var data = _dapper.Query<userDto>("select * from sys_user where Id=@id"
                ,new {id})
                .FirstOrDefault(); 
            return data;
        }

        public async Task< userDto> checkLogin(string username, string password)
        {
            var user = await _dapper.QueryAsync<userDto>(@"select * from sys_user where account=@username and password=@password",
                new { username, password }, commandType: CommandType.Text);

            if ( user==null || user.Count()==0 ) throw Oops.Oh(ErrorCode.D1000);
            return user.FirstOrDefault();
        }

        public List<RoleOutput> GetUserRoleList(long userId)
        {
            return _dapper.Query<RoleOutput>(@"select a.* from sys_role a
inner join sys_user_role b on b.role_id=a.Id
where b.user_id=@userId",
                new { userId }, commandType: CommandType.Text).ToList();
        }
    }
}

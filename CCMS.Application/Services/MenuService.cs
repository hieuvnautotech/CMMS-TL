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

namespace CCMS.Application.Services
{
    public interface IMenuService
    {
        Task<List<MenuDesignTreeNode>> GetLoginMenusDesign(long userId);
        void InsertMenu(Add_Menu_input model);
        Task UpdateMenu(UpdateMenuInput input);
        
        Task DeleteMenu(long? menu_id);
        dynamic GetMenuList([FromQuery] GetMenuListInput input);

        Task<List<string>> GetAllPermission();
        Task<List<string>> GetLoginPermissionList(long userId);

        void InsertPermission(Add_Permission_input model);
        void Insert_Menu_Standarddb(Add_Permission_input2 model);

    }
    public class MenuService: IMenuService,ITransient
    {
        private readonly ISysCacheService _sysCacheService;
        private readonly IDapperRepository<SysMenu> _dapper;

        public MenuService(IDapperRepository<SysMenu> dapperRepository, ISysCacheService sysCacheService)
        {
            _dapper = dapperRepository;
            _sysCacheService = sysCacheService;
        }

        public dynamic GetMenuList([FromQuery] GetMenuListInput input)
        {

            var name =input.Name?.Trim();
            var menus = _dapper.Context.Query<MenuOutput>(@"select  * from sys_menu where @name is null or name like '%' + @name + '%' ", new { name  })
               .ToList();
            var res = new TreeBuildUtil<MenuOutput>().Build(menus);
            return res;
        }

        private string CreateNewPids(long? pid)
        {
            if (pid == 0L || pid==null)
            {
                return "[0],";
            }
            else
            {

                var pmenu = _dapper.Query<SysMenu>(@"select  * from sys_menu where id=@pid", new {pid}).FirstOrDefault(); ;
                if (pmenu != null)
                {
                    return pmenu.Pids + "[" + pid + "],";
                }
                return null;
                
            }
        }

        public void InsertMenu(Add_Menu_input model)
        {
          //  var userId = UserManager.UserId;
          //  var userName = UserManager.Name;

          var  Id = YitIdHelper.NextId();
           var CreatedTime = DateTime.Now;
            var Pids =  CreateNewPids(model.Pid);
            _dapper.Execute(@"
                    INSERT INTO sys_menu (
                        Id,
                         Name,
                         Code,
                         Type,
                         Icon,
                         Router,
                         Component,
                         Permission,
                         Visible,
                         IsDeleted,
                         CreatedTime,
                         Sort,
                        PId,
                        Pids,
                        Title
                     )
                     VALUES (
                         @id,
                         @name,
                         @code,
                         1,
                         null,
                         @router,
                         @component,
                         'sys_' + @code + ':view',
                         1,
                         0,
                         getdate(),
                         10,
                            @pid,
                            @pids,
                            @title
                     ); ", new {title=model.Title,  id=Id,name= model.Name,code= model.Code, router=model.Router, component=model.Component, pid=model.Pid,pids= Pids });

        }

        public void Insert_Menu_Standarddb(Add_Permission_input2 model)
        {
            //  var userId = UserManager.UserId;
            //  var userName = UserManager.Name;

            var Id = YitIdHelper.NextId();
            var CreatedTime = DateTime.Now;
            long? Pid = 301596543320133;
            string Pids = CreateNewPids(Pid);

            _dapper.Execute(@"
                    INSERT INTO sys_menu (
                        Id,
                         Name,
                         Code,
                         Type,
                         Icon,
                         Router,
                         Component,
                         Permission,
                         Visible,
                         IsDeleted,
                         CreatedTime,
                         Sort,
                        PId,
                        Pids
                     )
                     VALUES (
                         @id,
                         @name,
                         @code,
                         1,
                         null,
                         @router,
                         @component,
                         'sys_' + @code + ':view',
                         1,
                         0,
                         getdate(),
                         10,
                            @pid,
                            @pids
                     ); ", new { id = Id, name = model.Name,code=model.Code,  router = model.Router, component = model.Component, pid = Pid, pids = Pids });

        }


        public void InsertPermission(Add_Permission_input model)
        {
           
            var Id = YitIdHelper.NextId();
            var CreatedTime = DateTime.Now;
            var Pids = CreateNewPids(model.Pid);
            _dapper.Execute(@"
                    INSERT INTO sys_menu (
                        Id,
                         Name,
                         Code,
                         Type,
                         Icon,
                         Router,
                         Component,
                         Permission,
                         Visible,
                         IsDeleted,
                         CreatedTime,
                         Sort,
                        PId,
                        Pids
                     )
                     VALUES (
                         @id,
                         @name,
                         @code,
                         2,
                         null,
                         null,
                         null,
                         @name,
                         1,
                         0,
                         getdate(),
                         10,
                            @pid,
                            @pids
                     ); ", new { id = Id, name = model.Name, code = model.Code,  pid = model.Pid, pids = Pids });

        }

       

        public async Task UpdateMenu(UpdateMenuInput input)
        {
           
            // PID and ID cannot be consistent, which will cause infinite recursive
            if (input.Id == input.Pid)
                throw Oops.Oh(ErrorCode.D4006);

            var ExistObj = _dapper.Context.Query<SysMenu>(@"select  * from sys_menu where code=@code and id != @id", new {@code=input.Code, id = input.Id }).FirstOrDefault();
            if (ExistObj !=null)
                throw Oops.Oh(ErrorCode.D4000);

            //kiểm tra ParentId nếu có
            if (input.Pid !=null)
            {
                var ExistParentObj = _dapper.Context.Query<SysMenu>(@"select * from sys_menu where id != @id", new { id = input.Pid }).FirstOrDefault();
                if (ExistParentObj == null)
                    throw Oops.Oh(ErrorCode.D4009);
            }

           // CheckMenuParam(input);
            var menuList = new List<SysMenu>();

            // If it is edit, the father ID cannot be for his own sub -node
            var childIdList =  _dapper.Query<long>(@"select id from sys_menu where Pids like '%' +  @id + '%'", new {id=input.Id.ToString()}).ToList();//.Where(u => u.Pids.Contains(input.Id.ToString()))
                                                               
            if (input.Pid !=null && childIdList.Contains(input.Pid.Value))
                throw Oops.Oh(ErrorCode.D4006);

            var oldMenu = await _dapper.Context.QueryFirstAsync<SysMenu>(@"select * from sys_menu where id=@id", new {id=input.Id});// (u => u.Id == input.Id);

            // Newpids
            var newPids =  CreateNewPids(input.Pid);

            // Whether to update the sign of sub-application
           // var updateSubAppsFlag = false;
            // Whether to update the pids logo of the sub-node
            var updateSubPidsFlag = false;

         
            if (input.Pid != oldMenu.Pid)
                updateSubPidsFlag = true;

            // Start update the configuration of all sub-nodes
            if ( updateSubPidsFlag)
            {
                // Find all leaf nodes, including sub-nodes that include sub-nodes
                menuList =  _dapper.Context.Query<SysMenu>(@"select * from sys_menu where Pids like '%' + @id + '%'", new { id=input.Id.ToString()}).ToList();// _sysMenuRep.Where(u => u.Pids.Contains(oldMenu.Id.ToString())).ToListAsync();
                // Update the application of all sub-nodes to the application of the current menu
                if (menuList.Count > 0)
                {
                    // Update all sub -nodes pids
                    if (updateSubPidsFlag)
                    {
                        menuList.ForEach(u =>
                        {
                            // Subnode pids  = current menu new pids + current menu id + subnode's own pids suffix
                            var oldParentCodesPrefix = oldMenu.Pids + "[" + oldMenu.Id + "],";
                            var oldParentCodesSuffix = u.Pids.Substring(oldParentCodesPrefix.Length);
                            var menuParentCodes = newPids + "[" + oldMenu.Id + "]," + oldParentCodesSuffix;
                            u.Pids = menuParentCodes;
                        });
                    }
                }
            }

            // Update the current menu
            oldMenu.Name = input.Name;
            oldMenu.Router = input.Router;
            oldMenu.Code = input.Code;
            oldMenu.Pid = input.Pid;
            oldMenu.Pids = newPids;
            oldMenu.Component = input.Component;
            using (var tran = _dapper.Context.BeginTransaction())
            {
                menuList.Add(oldMenu);
                foreach (var w in menuList)
                {
                    _dapper.Update(w,tran);
                }


                //oldMenu.Name = "test9999";
                //_dapper.Update(oldMenu);
              
                tran.Commit();
            }
              

        

        }

      
        public async Task DeleteMenu(long? menu_id)
        {
           if (menu_id==null) throw Oops.Oh(ErrorCode.D4008);
            var childIdList = _dapper.Query<long>(@"select id from sys_menu where Pids like @menu_id + '%' ", new { menu_id=menu_id.Value.ToString() }).ToList();
                                                               
            childIdList.Add(menu_id.Value);

          using (var tran = _dapper.Context.BeginTransaction())
            {
                await _dapper.ExecuteAsync(@"delete  from sys_menu where id  in @childIdList ", new { childIdList },tran);
                // Class joint delete the character corresponding to the menu and sub-menu-menu table information
                await _dapper.ExecuteAsync(@"delete from sys_role_menu where menu_id  in @childIdList ", new { childIdList },tran);
                tran.Commit();
            }
         
        
        }

        public async Task<List<MenuDesignTreeNode>> GetLoginMenusDesign(long userId)
        {
            var sysMenuList = new List<SysMenu>();

            if (UserManager.IsSuperAdmin)
            {
               var sysMenus = await _dapper.QueryAsync<SysMenu>("select * from sys_menu where type != @type and  (IsDeleted is null or IsDeleted !=1) "
                , new { type = (int)MenuType.BTN });
                sysMenuList = sysMenus.ToList();

            } else
            {
                var sysRoles = _dapper.Query<long>(@"select a.* from sys_role a
                            inner join sys_user_role b on b.role_id=a.Id
                            where b.user_id=@userId",
                             new { userId }, commandType: CommandType.Text).ToList();
                var menuIdList = _dapper.Query<long>(@"select menu_id from sys_role_menu
                            where role_id in @list_roles",
                             new { list_roles= sysRoles }, commandType: CommandType.Text).ToList();

                var sysMenus = await _dapper.QueryAsync<SysMenu>("select * from sys_menu where type != @type and  (IsDeleted is null or IsDeleted !=1) and id in @lst_menu_ids  "
              , new { type = (int)MenuType.BTN, lst_menu_ids=menuIdList }, commandType: CommandType.Text);
                sysMenuList = sysMenus.ToList();
            }
        

             var   menuDesignTreeNodes = sysMenuList
                .OrderBy(a=>a.Sort)
                .Select(u => new MenuDesignTreeNode
             {
                    
                    Id = u.Id,
                   Pid=u.Pid,
                    Name = u.Name,
                    Component = u.Component,
                    Router=u.Router,
                     IsShowDefault=u.IsShowDefault,
                      Visiable=u.Visible,
                      Title=u.Title,
                      Icon=u.Icon
                    //Meta = new Meta
                    //{
                    //    Title = u.Name,
                    //    Icon = u.Icon,
                    //    Show = u.Visible == YesOrNot.Y.ToString(),
                    //     Router = u.Router,
                        
                    //}
                }).ToList();
              
            
            return menuDesignTreeNodes;
        }

        public async Task<List<string>> GetAllPermission()
        {
            var permissions = await _sysCacheService.GetAllPermission();  
            if (permissions == null || permissions.Count < 1)
            {
                var res = await _dapper.Context.QueryAsync<string>(@"select Permission from sys_menu where type=@type", new { type = (int)MenuType.BTN });
                  permissions = res.ToList();

                await _sysCacheService.SetAllPermission(permissions);  
            }

            return permissions;
        }


        public async Task<List<string>> GetLoginPermissionList(long userId)
        {
            var permissions = await _sysCacheService.GetPermission(userId);
            if (permissions == null || permissions.Count < 1)
            {
                if (!UserManager.IsSuperAdmin)
                {
                    var  lst =  App.GetService<IUserService>().GetUserRoleList(userId);
                    var roleIdList = lst.Select(a => a.Id).ToList();
                    var menuIdList = _dapper.Query<long>(@"select menu_id from sys_role_menu
                            where role_id in @list_roles",
                             new { list_roles = roleIdList }, commandType: CommandType.Text).ToList();

                   
                    permissions = _dapper.Query<string>(@"select Permission from sys_menu
                            where type=@type and ( id in @list )",
                             new { list = menuIdList, type= (int)MenuType.BTN }, commandType: CommandType.Text).ToList();

                }
                else
                {
                    permissions = _dapper.Query<string>(@"select Permission from sys_menu
                            where type=@type",
                              new {   type = (int)MenuType.BTN }, commandType: CommandType.Text).ToList();
                }
                await _sysCacheService.SetPermission(userId, permissions);
            }
            return permissions;
        }

    }
}

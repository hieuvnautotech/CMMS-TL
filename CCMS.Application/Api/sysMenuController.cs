using CCMS.Application.Dtos;
using CCMS.Application.Services;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Application.Api
{
   
    [Route("api/[controller]")]
    [ApiDescriptionSettings("Menu manager",Tag = "Quản lý menu")]
   
    public class sysMenuController : ControllerBase
    {
        //private readonly IDapperRepository _dapperRepository;

        private readonly IMenuService _menuSvr;
        public sysMenuController(IMenuService menuSvr)
        {
            _menuSvr = menuSvr;
        }


        [HttpPost("add-menu")]
        [AllowAnonymous]
        public async Task Add_menu([Required] Add_Menu_input input)
        {
            
            _menuSvr.InsertMenu(input);
            await Task.CompletedTask;
            
        }
        [HttpPost("update-menu")]
        [AllowAnonymous]
      
        public async Task Update_menu([Required] UpdateMenuInput input)
        {

           await _menuSvr.UpdateMenu(input);
           
        }


        [HttpPost("Insert-Menu-Standarddb")]
        [AllowAnonymous]
        public async Task Insert_Menu_Standarddb([Required] Add_Permission_input2 input)
        {
            _menuSvr.Insert_Menu_Standarddb(input);
            await Task.CompletedTask;

        }


        [HttpPost("delete-menu")]
        [AllowAnonymous]
        public async Task Delete_menu([Required] long? menu_id)
        {

            await _menuSvr.DeleteMenu(menu_id);

        }

        [HttpGet("list")]
        public dynamic GetMenuList([FromQuery] GetMenuListInput input)
        {

            return _menuSvr.GetMenuList(input);
        }


        [HttpPost("add-permission")]
        [AllowAnonymous]
        public async Task Add_permission([Required] Add_Permission_input input)
        {

            _menuSvr.InsertPermission(input);
            await Task.CompletedTask;

        }
    }
}

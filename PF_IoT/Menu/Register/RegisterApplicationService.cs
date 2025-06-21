using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PF.Core.Entity;

namespace PF_IoT
{
    public class RegisterApplicationService : IRegisterApplicationService
    {
        private ISys_menuServices _Sys_menuService;

        public RegisterApplicationService(ISys_menuServices Sys_menuService)
        {
            this._Sys_menuService = Sys_menuService;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void initRegister()
        {
            List<Sys_menu> list = new List<Sys_menu>();
            FunctionManager.getFunctionLists().ForEach(item =>
            {
                list.Add(new Sys_menu()
                {
                    //Action = item.Action,
                    //Controller = item.Controller,
                    //CssClass = item.CssClass,
                    //FatherResource = item.FatherResource,
                    //IsMenu = item.IsMenu,
                    //Name = item.Name,
                    //RouteName = item.RouteName,
                    //SysResource = item.SysResource,
                    //Sort = item.Sort,
                    //FatherID = item.FatherID,
                    //IsDisabled = false,
                    //ResouceID = item.ResouceID
                    MenuName = item.Action
                    //public long MenuId { get; set; }
                    //public string MenuName { get; set; }
                    //public string MenuUrl { get; set; }
                    //public string MenuIcon { get; set; }
                    //public long? MenuParent { get; set; }
                    //public int? Sort { get; set; }
                    //public byte? Status { get; set; }
                    //public string MenuType { get; set; }
                    //public byte? IsDel { get; set; } = 1;
                    //public string Remark { get; set; }
                });
                _Sys_menuService.initMenu(list);
            });
        }
    }
}

using System.Collections.Generic;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Table;

namespace IServices
{
    public interface ISys_userServices : IBaseServices<Sys_user>
    {
        (bool, string, SysUserDto) CheckLogin(SysUserDto dto);

        string PageList(Bootstrap.BootstrapParams bootstrap);

        void Login(long userId, string ip);
    }
}
using System.Collections.Generic;
using PF.Core.Entity;

namespace IServices
{
    public interface ISys_menuServices : IBaseServices<Sys_menu>
    {
        void initMenu(List<Sys_menu> list);
    }
}
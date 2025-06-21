using IRepository;
using IServices;
using System.Collections.Generic;
using PF.Core.Entity;

namespace Services
{
    public class Sys_menuServices : BaseServices<Sys_menu>, ISys_menuServices
    {
        public Sys_menuServices(ISys_menuRepository repository) : base(repository)
        {
        }
        //Owen
        public void  initMenu( List<Sys_menu> list)
        {
           
        }

    }
}